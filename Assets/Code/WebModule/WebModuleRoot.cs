using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleRoot : MonoBehaviour, IFocusable
    {
        private const string UpgradeManagerJsonTokenName = "upgrades";
        private const string CommonSpidersLevelsJsonTokenName = "commonSpidersLevels";
        private const string SpiderStatesJsonTokenName = "spiderStates";
        
        [field: SerializeField] public WebModuleData Data { get; private set; }
        [SerializeField] private GameObject mainSpiderPrefab;
        [SerializeField] private GameObject commonSpiderPrefab;
        [SerializeField] private GameObject normalFoodPrefab;
        [SerializeField] private GameObject bigFoodPrefab;
        [field: SerializeField] public UpgradeManager UpgradeManager { get; private set; }
        [field: SerializeField] public WebModuleUI WebModuleUI { get; private set; }
        [SerializeField] private Transform cameraPosition;

        public WebModuleFacade Facade;
        public readonly List<Spider> Spiders = new List<Spider>();
        public readonly List<Food> _foods = new List<Food>();
        private Spider _mainSpider;
        private float _timeUntilNewWave;

        public long SilkInInventory => Facade.Inventory.Silk;

        public void Initialize(WebModuleFacade facade, JObject saveData = null)
        {
            SpawnMainSpider();
            _timeUntilNewWave = Data.FoodWaveInterval;
            Facade = facade;
            UpgradeManager = new UpgradeManager(this, saveData?[UpgradeManagerJsonTokenName]?.ToObject<JObject>());
            WebModuleUI.Initialize(this);
            if (saveData?[CommonSpidersLevelsJsonTokenName] != null)
            {
                int[] commonSpidersLevels = saveData[CommonSpidersLevelsJsonTokenName].ToObject<int[]>();
                foreach (int i in commonSpidersLevels)
                {
                    var spider = SpawnCommonSpider();
                    spider.Level = i;
                }
            }

            if (saveData?[SpiderStatesJsonTokenName] != null)
            {
                Spider.StateEnum[] states = saveData?[SpiderStatesJsonTokenName].ToObject<Spider.StateEnum[]>();
                for (int i = 0; i < states.Length; i++)
                {
                    Spiders[i].State = states[i];
                }
            }
        }
        
        private void SpawnFood(bool _isBig)
        {
            var prefab = _isBig ? bigFoodPrefab : normalFoodPrefab;
            Food food = Instantiate(prefab, transform).GetComponent<Food>();
            food.Initialize(this, _isBig);
            _foods.Add(food);
            food.EDeath += OnFoodDeath;
            food.EEscape += OnFoodEscape;
            food.transform.position = GetRandomPosition();
        }
        
        private Vector2 GetRandomPosition()
        {
            Vector2 position;
            position.x = Random.Range(-Data.WebWidth / 2, Data.WebWidth / 2);
            position.y = Random.Range(-Data.WebHeight / 2, Data.WebHeight / 2);
            return position;
        }
        
        private void OnFoodDeath(Food food)
        {
            OnFoodDeletion(food);
        }

        private void OnFoodEscape(Food food)
        {
            OnFoodDeletion(food);
        }
        
        private void OnFoodDeletion(Food food)
        {
            food.EDeath -= OnFoodDeath;
            food.EEscape -= OnFoodEscape;
            _foods.Remove(food);
        }

        private void FixedUpdate()
        {
            if (_timeUntilNewWave <= 0)
            {
                SpawnFoodWave();
                _timeUntilNewWave = Data.FoodWaveInterval;
            }

            _timeUntilNewWave -= Time.fixedDeltaTime;
        }

        private void SpawnFoodWave()
        {
            float foodSpawnChance = UpgradeManager.FoodSpawnChanceUpgrade.TotalChance;
            int guaranteedFoodAmount = Mathf.FloorToInt(foodSpawnChance);
            float extraFoodChance = foodSpawnChance - guaranteedFoodAmount;
            bool addExtraFood = Random.Range(0f, 1f) <= extraFoodChance;
            int foodAmount = guaranteedFoodAmount + (addExtraFood ? 1 : 0);
            for (int i = 0; i < foodAmount; i++)
            {
                bool isBig = Data.BigFoodBaseChance >= Random.Range(0f, 1f);
                SpawnFood(isBig);
            }
        }

        private void SpawnMainSpider()
        {
            _mainSpider = Instantiate(mainSpiderPrefab, transform).GetComponent<Spider>();
            _mainSpider.Initialize(this, true);
            _mainSpider.transform.position = GetRandomPosition();
            Spiders.Add(_mainSpider);
        }

        public Spider SpawnCommonSpider()
        {
            var spider = Instantiate(commonSpiderPrefab, transform).GetComponent<Spider>();
            spider.Initialize(this, false);
            spider.transform.position = GetRandomPosition();
            Spiders.Add(spider);
            return spider;
        }
        
        public void SpendSilk(long amount)
        {
            if (SilkInInventory < amount)
            {
                throw new Exception("Not enough silk");
            }

            Facade.Inventory.Silk -= amount;
        }

        public void Tick(float deltaTime)
        {
            
        }

        public JObject SaveModuleToJson()
        {
            return new JObject
            (
                new JProperty(UpgradeManagerJsonTokenName, UpgradeManager.SaveToJson()),
                new JProperty(CommonSpidersLevelsJsonTokenName, 
                    new JArray(Spiders.Where(x => !x.IsMainSpider).Select(x => x.Level))),
                new JProperty(SpiderStatesJsonTokenName, new JArray(Spiders.Select(x => x.State)))
            );
        }

        public void OnGetFocus()
        {
            WebModuleUI.gameObject.SetActive(true);
        }

        public void OnLoseFocus()
        {
            WebModuleUI.gameObject.SetActive(false);
        }

        public Vector3 GetCameraPosition()
        {
            return cameraPosition.position;
        }
    }
}