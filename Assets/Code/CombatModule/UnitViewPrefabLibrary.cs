using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    [Serializable]
    public class UnitViewPrefabLibrary
    {
        [SerializeField] private GameObject mainSpider;
        [SerializeField] private GameObject endlessFightEnemy;
        [SerializeField] private GameObject testUnit;

        private Dictionary<Type, GameObject> _dictionary;
        
        public GameObject GetPrefab(Unit unit) => GetPrefab(unit.GetType());

        public GameObject GetPrefab(Type type)
        {
            if (!_dictionary.ContainsKey(type))
            {
                throw new ArgumentException($"{nameof(UnitViewPrefabLibrary)} doesn't contain a view prefab for " +
                                            $"{type.ToString().Split('.').Last()}");
            }

            return _dictionary[type];
        }

        public void Initialize()
        {
            _dictionary = new Dictionary<Type, GameObject>
            {
                {typeof(EndlessFightEnemy), endlessFightEnemy},
                {typeof(MainSpider), mainSpider},
                {typeof(TestUnit), testUnit}
            };
        }
        
    }
}