using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleRoot : MonoBehaviour
    {
        public CombatModuleFacade Facade { get; private set; }

        [SerializeField] private Transform[] _allyPositions;
        [SerializeField] private Transform[] _enemyPositions;
        [SerializeField] private UnitViewPrefabLibrary unitViewPrefabs;
        [SerializeField] private EndlessFightData _endlessFightData;
        
        private bool _isCombatActive;
        private IEncounter _currentEncounter;
        private int _currentWaveNumber;
        private MainSpider _mainSpider;

        private List<Unit> _allies = new List<Unit>();
        private List<Unit> _enemies = new List<Unit>();

        private IEnumerable<UnitView> AllyViews => _allies.Select(x => x.View);
        private IEnumerable<UnitView> EnemyViews => _enemies.Select(x => x.View);

        private IEnumerable<Unit> Units => _allies.Concat(_enemies);
        private IEnumerable<UnitView> UnitViews => AllyViews.Concat(EnemyViews);
        private bool[] _areAllyPositionsTaken;
        private bool[] _areEnemyPositionsTaken;

        public void Initialize(CombatModuleFacade facade, JObject saveData)
        {
            Facade = facade;
            unitViewPrefabs.Initialize();
            _areAllyPositionsTaken = new bool[_allyPositions.Length];
            _areEnemyPositionsTaken = new bool[_enemyPositions.Length];
            StartEncounter(new EndlessFight(this, _endlessFightData));
        }

        private void StartEncounter(IEncounter encounter)
        {
            Debug.Log("Combat started"); // TODO remove
            _isCombatActive = true;
            _currentEncounter = encounter;
            SpawnMainSpider();
            GoToWave(1);
        }

        private void GoToWave(int waveNumber)
        {
            _currentWaveNumber = waveNumber;
            Wave wave = _currentEncounter.GetWave(waveNumber);
            ClearEnemies();
            foreach (var enemy in wave)
            {
                SpawnUnit(enemy);
            }
        }

        private void GoToNextWave() => GoToWave(_currentWaveNumber + 1);

        private void ClearUnitsInList(ref List<Unit> list)
        {
            foreach (var unit in list)
            {
                unit.Death -= OnUnitDeath;
                Destroy(unit.View.gameObject);
            }
            
            list = new List<Unit>();
        }

        private void ClearEnemies() => ClearUnitsInList(ref _enemies);

        private void ClearAllies() => ClearUnitsInList(ref _allies);

        public void Tick(float deltaTime)
        {
            if (_isCombatActive)
            {
                foreach (var unit in Units)
                {
                    unit.Tick(deltaTime);
                }
                
                if (_enemies.Count == 0)
                {
                    if (_currentWaveNumber == _currentEncounter.LastWaveNumber)
                    {
                        EndCombat();
                    }
                    else
                    {
                        GoToNextWave();
                    }
                }

                if (_allies.Count == 0)
                {
                    EndCombat();
                }
            }
        }

        private void SpawnUnit(Unit unit)
        {
            UnitView view = Instantiate(unitViewPrefabs.GetPrefab(unit), transform).GetComponent<UnitView>();
            unit.View = view;
            view.Unit = unit;
            Transform[] positions;
            bool[] arePositionsTaken;
            switch (unit.Side)
            {
                case Side.Allies:
                    positions = _allyPositions;
                    arePositionsTaken = _areAllyPositionsTaken;
                    break;
                case Side.Enemies:
                    positions = _enemyPositions;
                    arePositionsTaken = _areEnemyPositionsTaken;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            bool positioningSuccess = false;
            for (int i = 0; i < positions.Length; i++)
            {
                if (!arePositionsTaken[i])
                {
                    view.transform.position = positions[i].position;
                    arePositionsTaken[i] = true;
                    view.PositionIndex = i;
                    positioningSuccess = true;
                    break;
                }
            }
            if (!positioningSuccess)
            {
                throw new Exception("Can't find a position for a unit view");
            }

            unit.Death += OnUnitDeath;
            
            switch (unit.Side)
            {
                case Side.Allies:
                    _allies.Add(unit);
                    break;
                case Side.Enemies:
                    _enemies.Add(unit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SpawnMainSpider()
        {
            _mainSpider = new MainSpider(this);
            SpawnUnit(_mainSpider);
        }

        private void EndCombat()
        {
            Debug.Log("Combat finished"); // TODO remove
            _isCombatActive = false;
            ClearAllies();
            ClearEnemies();
            // TODO go to adventure map
        }

        private void OnUnitDeath(Unit unit)
        {
            unit.Death -= OnUnitDeath;
            
            switch (unit.Side)
            {
                case Side.Allies:
                    _allies.Remove(unit);
                    break;
                case Side.Enemies:
                    _enemies.Remove(unit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Destroy(unit.View.gameObject);

            bool[] arePositionsTaken;
            switch (unit.Side)
            {
                case Side.Allies:
                    arePositionsTaken = _areAllyPositionsTaken;
                    break;
                case Side.Enemies:
                    arePositionsTaken = _areEnemyPositionsTaken;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            arePositionsTaken[unit.View.PositionIndex] = false;
        }

        private Unit GetRandomUnitFromCollection(IEnumerable<Unit> collection)
        {
            return collection.ElementAtOrDefault(Random.Range(0, collection.Count()));
        }

        public Unit GetRandomAlly() => GetRandomUnitFromCollection(_allies);

        public Unit GetRandomEnemy() => GetRandomUnitFromCollection(_enemies);

        public Unit GetRandomUnit() => GetRandomUnitFromCollection(Units);
    }
}