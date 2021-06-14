using System.Collections.Generic;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

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

        public readonly List<Spider> _spiders = new List<Spider>();
        public readonly List<Food> _foods = new List<Food>();
        private Spider _mainSpider;

        public void Initialize(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _mainSpider = Instantiate(mainSpiderPrefab, transform).GetComponent<Spider>();
            _mainSpider.Initialize(this);
            _spiders.Add(_mainSpider);
            CreateNormalFood();
        }

        private void CreateNormalFood()
        {
            Food food = Instantiate(normalFoodPrefab, transform).GetComponent<Food>();
            food.Initialize();
            _foods.Add(food);
            food.Death += OnFoodDeath;
            food.Escape += OnFoodEscape;
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
            food.Death -= OnFoodDeath;
            food.Escape -= OnFoodEscape;
            _foods.Remove(food);
        }
    }
}