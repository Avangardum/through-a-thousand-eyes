using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleRoot : MonoBehaviour
    {
        public CombatModuleFacade Facade;
        
        [SerializeField] private Transform[] _allyPositions;
        [SerializeField] private Transform[] _enemyPositions;
        [SerializeField] private UnitView _unitViewPrefab;
        
        private bool _isCombatActive;
        private IEncounter _currentEncounter;
        private int _currentWaveNumber;

        private List<Unit> _allies = new List<Unit>();
        private List<Unit> _enemies = new List<Unit>();

        private List<UnitView> _allyViews = new List<UnitView>();
        private List<UnitView> _enemyViews = new List<UnitView>();

        private IEnumerable<Unit> Units => _allies.Concat(_enemies);
        private IEnumerable<UnitView> UnitViews => _allyViews.Concat(_enemyViews);

        public void Initialize(CombatModuleFacade facade, JObject saveData)
        {
            Facade = facade;
        }

        private void StartEncounter(IEncounter encounter)
        {
            _isCombatActive = true;
            _currentEncounter = encounter;
            GoToWave(1);
            // Spawn allies
        }

        private void GoToWave(int waveNumber)
        {
            _currentWaveNumber = waveNumber;
            Wave wave = _currentEncounter.GetWave(waveNumber);
            CleanEnemies();
            foreach (var enemy in wave)
            {
                SpawnEnemy(enemy);
            }
        }

        private void GoToNextWave() => GoToWave(_currentWaveNumber + 1);

        private void CleanEnemies()
        {
            _enemies = new List<Unit>();
            
            foreach (var enemyView in _enemyViews)
            {
                Destroy(enemyView);
            }

            _enemyViews = new List<UnitView>();
        }

        private void CleanAllies()
        {
            _allies = new List<Unit>();

            foreach (var allyView in _allyViews)
            {
                Destroy(allyView);
            }

            _allyViews = new List<UnitView>();
        }

        public void Tick()
        {
            if (_isCombatActive)
            {
                foreach (var unit in Units)
                {
                    unit.Tick();
                }
            }
        }

        private void SpawnEnemy(Unit enemy)
        {
            _enemies.Add(enemy);
            UnitView view = Instantiate(_unitViewPrefab).GetComponent<UnitView>();
            enemy.View = view;
            _enemyViews.Add(view);
        }
    }
}