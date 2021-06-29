using System;
using System.Collections.Generic;
using ThroughAThousandEyes.MainModule;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleRoot : MonoBehaviour
    {
        [field: SerializeField] public WebModuleData Data { get; private set; }
        [SerializeField] private GameObject mainSpiderPrefab;
        [SerializeField] private GameObject commonSpiderPrefab;
        [SerializeField] private GameObject normalFoodPrefab;
        [SerializeField] private GameObject bigFoodPrefab;
        [field: SerializeField] public UpgradeManager UpgradeManager { get; private set; }
        [field: SerializeField] public WebModuleUI WebModuleUI { get; private set; }

        public WebModuleFacade Facade;
        public readonly List<Spider> _spiders = new List<Spider>();
        public readonly List<Food> _foods = new List<Food>();
        private Spider _mainSpider;
        private float _timeUntilNewWave;

        public long SilkInInventory => Facade.Inventory.Silk;

        public void Initialize(WebModuleFacade facade, bool isLoadingSavedGame, string saveData = "")
        {
            SpawnMainSpider();
            _timeUntilNewWave = Data.FoodWaveInterval;
            Facade = facade;
            UpgradeManager = new UpgradeManager(this);
            WebModuleUI.Initialize(this);
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
            _spiders.Add(_mainSpider);
        }

        public void SpawnCommonSpider()
        {
            var spider = Instantiate(commonSpiderPrefab, transform).GetComponent<Spider>();
            spider.Initialize(this, false);
            spider.transform.position = GetRandomPosition();
            _spiders.Add(spider);
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
    }
}