using System;
using System.Collections.Generic;
using ThroughAThousandEyes.MainModule;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleRoot : MonoBehaviour
    {
        [SerializeField] private float webWidth = 7;
        [SerializeField] private float webHeight = 7;
        [SerializeField] private GameObject mainSpiderPrefab;
        [SerializeField] private GameObject commonSpiderPrefab;
        [SerializeField] private GameObject normalFoodPrefab;
        [SerializeField] private GameObject bigFoodPrefab;
        [SerializeField] private float foodWaveInterval = 1;
        [field: SerializeField] public float AttackRange { get; private set; } = 0.5f;
        [field: SerializeField] public float AttackInterval { get; private set; } = 1f;
        [field: SerializeField] public decimal ExperienceToLevelUpBase { get; private set; } = 100;
        [field: SerializeField] public decimal ExperienceToLevelUpAddition { get; private set; } = 10;

        public readonly List<Spider> _spiders = new List<Spider>();
        public readonly List<Food> _foods = new List<Food>();
        private Spider _mainSpider;
        private float _timeUntilNewWave;
        private float _foodSpawnChance = 0.5f;

        public void Initialize(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _mainSpider = Instantiate(mainSpiderPrefab, transform).GetComponent<Spider>();
            _mainSpider.Initialize(this);
            _spiders.Add(_mainSpider);
            _timeUntilNewWave = foodWaveInterval;
        }

        private void SpawnNormalFood()
        {
            Food food = Instantiate(normalFoodPrefab, transform).GetComponent<Food>();
            food.Initialize();
            _foods.Add(food);
            food.EDeath += OnFoodDeath;
            food.EEscape += OnFoodEscape;
            food.transform.position = GetRandomPosition();
        }

        private Vector2 GetRandomPosition()
        {
            Vector2 position;
            position.x = Random.Range(-webWidth / 2, webWidth / 2);
            position.y = Random.Range(-webHeight / 2, webHeight / 2);
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
                _timeUntilNewWave = foodWaveInterval;
            }

            _timeUntilNewWave -= Time.fixedDeltaTime;
        }

        private void SpawnFoodWave()
        {
            int guaranteedFoodAmount = Mathf.FloorToInt(_foodSpawnChance);
            float extraFoodChance = _foodSpawnChance - guaranteedFoodAmount;
            bool addExtraFood = Random.Range(0f, 1f) <= extraFoodChance;
            int foodAmount = guaranteedFoodAmount + (addExtraFood ? 1 : 0);
            for (int i = 0; i < foodAmount; i++)
            {
                SpawnNormalFood();
            }
        }
    }
}