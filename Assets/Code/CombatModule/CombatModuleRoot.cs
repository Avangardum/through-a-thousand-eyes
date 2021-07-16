using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleRoot : MonoBehaviour, IFocusable
    {
        public CombatModuleFacade Facade { get; private set; }

        [SerializeField] private Transform cameraPosition;
        [SerializeField] private int frontLineCapacity = 3;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> allyFrontPositions;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> enemyFrontPositions;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> allyBackPositions;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> enemyBackPositions;
        [SerializeField] private UnitViewPrefabLibrary unitViewPrefabs;
        [SerializeField] private EndlessFightData endlessFightData;
        [Header("Logging")]
        [SerializeField] private bool logToConsole;
        
        private bool _isCombatActive;
        private IEncounter _currentEncounter;
        private int _currentWaveNumber;
        private MainSpider _mainSpider;

        private List<Unit> _frontAllies = new List<Unit>();
        private List<Unit> _frontEnemies = new List<Unit>();
        private List<Unit> _backAllies = new List<Unit>();
        private List<Unit> _backEnemies = new List<Unit>();
        
        private IEnumerable<Unit> FrontUnits => _frontAllies.Concat(_frontEnemies);
        private IEnumerable<Unit> BackUnits => _backAllies.Concat(_backEnemies);

        private IEnumerable<Unit> AllAllies => _frontAllies.Concat(_backAllies);

        private IEnumerable<Unit> AllEnemies => _frontEnemies.Concat(_backEnemies);
        private IEnumerable<Unit> AllUnits => AllAllies.Concat(AllEnemies);
        private bool IsAllyFrontLineOccupied => _frontAllies.Count >= frontLineCapacity;
        private bool IsEnemyFrontLineOccupied => _frontEnemies.Count >= frontLineCapacity;

        public void Initialize(CombatModuleFacade facade, JObject saveData)
        {
            Facade = facade;
            unitViewPrefabs.Initialize();
        }

        private void StartEncounter(IEncounter encounter)
        {
            Log("Combat started");
            _isCombatActive = true;
            _currentEncounter = encounter;
            SpawnMainSpider();
            GoToWave(1);
        }

        private void GoToWave(int waveNumber)
        {
            Log($"Wave {waveNumber} started");
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

        private void ClearEnemies()
        {
            ClearUnitsInList(ref _frontEnemies);
            ClearUnitsInList(ref _backEnemies);
        }

        private void ClearAllies()
        {
            ClearUnitsInList(ref _frontAllies);
            ClearUnitsInList(ref _backAllies);
        }

        // TODO adapt for the back line
        public void Tick(float deltaTime)
        {
            if (_isCombatActive)
            {
                foreach (var unit in AllUnits.ToList())
                {
                    unit.Tick(deltaTime);
                }

                if (_backAllies.Any() || _frontAllies.Count < frontLineCapacity)
                {
                    // TODO Move ally from back to front
                }

                if (_backEnemies.Any() || _frontEnemies.Count < frontLineCapacity)
                {
                    // TODO Move enemy from back to front
                }
                
                if (!AllEnemies.Any())
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

                if (!AllAllies.Any())
                {
                    EndCombat();
                }
            }
        }

        private void SpawnUnit(Unit unit)
        {
            unit.Death += OnUnitDeath;
                
            List<Unit> front;
            List<Unit> back;
            bool isFront;
            switch (unit.Side)
            {
                case Side.Allies:
                    front = _frontAllies;
                    back = _backAllies;
                    isFront = !IsAllyFrontLineOccupied;
                    break;
                case Side.Enemies:
                    front = _frontEnemies;
                    back = _backEnemies;
                    isFront = !IsEnemyFrontLineOccupied;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            List<Unit> targetList = isFront ? front : back;
            targetList.Add(unit);
            unit.IsOnFrontLine = isFront;
            
            UnitView view = Instantiate(unitViewPrefabs.GetPrefab(unit), transform).GetComponent<UnitView>();
            unit.View = view;
            view.Unit = unit;
            ArrayWithOccupiedFlags<Transform> positions;
            switch (unit.Side)
            {
                case Side.Allies:
                    positions = isFront ? allyFrontPositions : allyBackPositions;
                    break;
                case Side.Enemies:
                    positions = isFront ? enemyFrontPositions : enemyBackPositions;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            bool positioningSuccess = false;
            for (int i = 0; i < positions.Length; i++)
            {
                if (!positions.IsElementOccupied(i))
                {
                    view.transform.position = positions[i].position;
                    positions.MarkSlotAsOccupied(i);
                    view.PositionIndex = i;
                    positioningSuccess = true;
                    break;
                }
            }
            if (!positioningSuccess)
            {
                throw new Exception("Can't find a position for a unit view");
            }
        }
        
        private void SpawnMainSpider()
        {
            _mainSpider = new MainSpider(this);
            SpawnUnit(_mainSpider);
        }

        private void EndCombat()
        {
            Log("Combat finished");
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
                    _frontAllies.Remove(unit);
                    break;
                case Side.Enemies:
                    _frontEnemies.Remove(unit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Destroy(unit.View.gameObject);

            ArrayWithOccupiedFlags<Transform> positions;
            switch (unit.Side)
            {
                case Side.Allies:
                    positions = unit.IsOnFrontLine ? allyFrontPositions : allyBackPositions;
                    break;
                case Side.Enemies:
                    positions = unit.IsOnFrontLine ? enemyFrontPositions : enemyBackPositions;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            positions.MarkSlotAsFree(unit.View.PositionIndex);
        }

        private Unit GetRandomUnitFromCollection(IEnumerable<Unit> collection)
        {
            return collection.ElementAtOrDefault(Random.Range(0, collection.Count()));
        }

        public Unit GetRandomFrontAlly() => GetRandomUnitFromCollection(_frontAllies);

        public Unit GetRandomFrontEnemy() => GetRandomUnitFromCollection(_frontEnemies);

        public Unit GetRandomFrontUnit() => GetRandomUnitFromCollection(FrontUnits);

        public void Log(string message)
        {
            if (logToConsole)
            {
                Debug.Log(message);
            }
        }

        public void Log(object message) => Log(message.ToString());
        public void OnGetFocus()
        {
            
        }

        public void OnLoseFocus()
        {
            
        }

        public Vector3 GetCameraPosition()
        {
            return cameraPosition.position;
        }

        public void StartEndlessFight()
        {
            StartEncounter(new EndlessFight(this, endlessFightData));
        }
    }
}