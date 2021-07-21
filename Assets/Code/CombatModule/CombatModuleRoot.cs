using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleRoot : MonoBehaviour, IFocusable
    {
        public CombatModuleFacade Facade { get; private set; }

        private bool _isCombatActive;
        private IEncounter _currentEncounter;
        private int _currentWaveNumber;
        private MainSpider _mainSpider;
        
        #region Serialized fields

        [SerializeField] private Transform cameraPosition;
        [SerializeField] private CombatModuleUI ui;
        [SerializeField] private int frontLineCapacity = 3;
        [SerializeField] private int backLineCapacity = 2;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> allyFrontPositions;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> enemyFrontPositions;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> allyBackPositions;
        [SerializeField] private ArrayWithOccupiedFlags<Transform> enemyBackPositions;
        [SerializeField] private UnitViewPrefabLibrary unitViewPrefabs;
        [SerializeField] private EndlessFightData endlessFightData;
        [Header("Logging")]
        [SerializeField] private bool logToConsole;
        
        #endregion

        #region Unit lists
        
        private List<Unit> _frontAllies = new List<Unit>();
        private List<Unit> _frontEnemies = new List<Unit>();
        private List<Unit> _backAllies = new List<Unit>();
        private List<Unit> _backEnemies = new List<Unit>();
        private List<Unit> _idleAllies = new List<Unit>();
        private List<Unit> _idleEnemies = new List<Unit>();
        
        #endregion

        public int IdleAlliesCount => _idleAllies.Count;
        public int IdleEnemiesCount => _idleEnemies.Count;

        #region Unit list concats

        private IEnumerable<Unit> FrontUnits => _frontAllies.Concat(_frontEnemies);
        private IEnumerable<Unit> BackUnits => _backAllies.Concat(_backEnemies);

        private IEnumerable<Unit> ActiveAllies => _frontAllies.Concat(_backAllies);
        private IEnumerable<Unit> ActiveEnemies => _frontEnemies.Concat(_backEnemies);
        private IEnumerable<Unit> ActiveUnits => ActiveAllies.Concat(ActiveEnemies);

        private IEnumerable<Unit> AllAllies => ActiveAllies.Concat(_idleAllies);
        private IEnumerable<Unit> AllEnemies => ActiveEnemies.Concat(_idleEnemies);
        private IEnumerable<Unit> AllUnits => AllAllies.Concat(AllEnemies);

        #endregion

        #region Is line occupied properties

        private bool IsFrontAlliesFull => _frontAllies.Count >= frontLineCapacity;
        private bool IsFrontEnemiesFull => _frontEnemies.Count >= frontLineCapacity;
        private bool IsBackAlliesFull => _backAllies.Count >= backLineCapacity;
        private bool IsBackEnemiesFull => _backEnemies.Count >= backLineCapacity;

        #endregion

        #region Private methods

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
                SpawnUnit(enemy);
        }

        private void GoToNextWave() => GoToWave(_currentWaveNumber + 1);

        private void ClearUnitsInList(ref List<Unit> list)
        {
            foreach (var unit in list)
            {
                unit.Death -= OnUnitDeath;
                if (!ReferenceEquals(unit.View, null))
                    Destroy(unit.View.gameObject);
            }
            
            list = new List<Unit>();
        }

        private void ClearEnemies()
        {
            ClearUnitsInList(ref _frontEnemies);
            ClearUnitsInList(ref _backEnemies);
            ClearUnitsInList(ref _idleEnemies);
        }

        private void ClearAllies()
        {
            ClearUnitsInList(ref _frontAllies);
            ClearUnitsInList(ref _backAllies);
            ClearUnitsInList(ref _idleAllies);
        }
        
        private void MoveUnitFromBackToFront(Unit unit)
        {
            List<Unit> back;
            List<Unit> front;
            switch (unit.Side)
            {
                case Side.Allies:
                    back = _backAllies;
                    front = _frontAllies;
                    break;
                case Side.Enemies:
                    back = _backEnemies;
                    front = _frontEnemies;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (front.Count >= frontLineCapacity) throw new Exception("Front line is full");
            bool removeSuccess = back.Remove(unit);
            if (!removeSuccess) throw new ArgumentException("Unit is not on the back line");
            front.Add(unit);
            unit.Line = Line.Front;

            ArrayWithOccupiedFlags<Transform> backPositions;
            ArrayWithOccupiedFlags<Transform> frontPositions;
            switch (unit.Side)
            {
                case Side.Allies:
                    backPositions = allyBackPositions;
                    frontPositions = allyFrontPositions;
                    break;
                case Side.Enemies:
                    backPositions = enemyBackPositions;
                    frontPositions = enemyFrontPositions;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            backPositions.MarkSlotAsFree(unit.View.PositionIndex);
            unit.View.PositionIndex = frontPositions.GetFirstFreeIndex();
            unit.View.transform.position = frontPositions[unit.View.PositionIndex].position;
            frontPositions.MarkSlotAsOccupied(unit.View.PositionIndex);
        }

        private void MoveUnitFromIdleToBack(Unit unit)
        {
            List<Unit> idle;
            List<Unit> back;
            switch (unit.Side)
            {
                case Side.Allies:
                    idle = _idleAllies;
                    back = _backAllies;
                    break;
                case Side.Enemies:
                    idle = _idleEnemies;
                    back = _backEnemies;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (back.Count >= backLineCapacity) throw new Exception("Back line is full");
            bool removeSuccess = idle.Remove(unit);
            if (!removeSuccess) throw new ArgumentException("Unit is not on the idle line");
            back.Add(unit);
            unit.Line = Line.Back;
            CreateViewForUnit(unit);
        }
        
        private void SpawnUnit(Unit unit)
        {
            unit.Death += OnUnitDeath;
            
            // Define front back and idle lists
            List<Unit> front;
            List<Unit> back;
            List<Unit> idle;
            switch (unit.Side)
            {
                case Side.Allies:
                    front = _frontAllies;
                    back = _backAllies;
                    idle = _idleAllies;
                    break;
                case Side.Enemies:
                    front = _frontEnemies;
                    back = _backEnemies;
                    idle = _idleEnemies;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Find a line for the unit and place it there
            List<Unit> targetList;
            if (front.Count < frontLineCapacity)
            {
                unit.Line = Line.Front;
                targetList = front;
            }
            else if (back.Count < backLineCapacity)
            {
                unit.Line = Line.Back;
                targetList = back;
            }
            else
            {
                unit.Line = Line.Idle;
                targetList = idle;
            }
            targetList.Add(unit);
            
            if(unit.Line != Line.Idle)
                CreateViewForUnit(unit);
        }

        private void CreateViewForUnit(Unit unit)
        {
            UnitView view = Instantiate(unitViewPrefabs.GetPrefab(unit), transform).GetComponent<UnitView>();
            unit.View = view;
            view.Initialize(unit);
            ArrayWithOccupiedFlags<Transform> positions = GetPositionsArray(unit);
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

        private ArrayWithOccupiedFlags<Transform> GetPositionsArray(Unit unit)
        {
            switch (unit.Side)
            {
                case Side.Allies:
                    switch (unit.Line)
                    {
                        case Line.Front:
                            return allyFrontPositions;
                        case Line.Back:
                            return allyBackPositions;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case Side.Enemies:
                    switch (unit.Line)
                    {
                        case Line.Front:
                            return enemyFrontPositions;
                        case Line.Back:
                            return enemyBackPositions;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
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
            Log("Combat finished");
            _isCombatActive = false;
            ClearAllies();
            ClearEnemies();
            Facade.MainModuleFacade.ActivitySwitcher.SwitchToAdventureMap();
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

            if (unit.Line != Line.Idle)
            {
                ArrayWithOccupiedFlags<Transform> positions = GetPositionsArray(unit);
                positions.MarkSlotAsFree(unit.View.PositionIndex);
            }
        }

        private Unit GetRandomUnitFromCollection(IEnumerable<Unit> collection)
        {
            return collection.ElementAtOrDefault(Random.Range(0, collection.Count()));
        }

        #endregion

        #region Public methods

        public void Initialize(CombatModuleFacade facade, JObject saveData)
        {
            Facade = facade;
            unitViewPrefabs.Initialize();
            allyFrontPositions.InitializeSerialized();
            allyBackPositions.InitializeSerialized();
            enemyFrontPositions.InitializeSerialized();
            enemyBackPositions.InitializeSerialized();
            ui.Initialize(this);
        }

        public void Tick(float deltaTime)
        {
            if (_isCombatActive)
            {
                foreach (var unit in ActiveUnits.ToList())
                {
                    unit.Tick(deltaTime);
                }

                // Move allies between lines
                bool wasAnyUnitMoved;
                do
                {
                    wasAnyUnitMoved = false;
                    if (_backAllies.Any() && !IsFrontAlliesFull)
                    {
                        MoveUnitFromBackToFront(_backAllies.First());
                        wasAnyUnitMoved = true;
                    }
                    if (_idleAllies.Any() && !IsBackAlliesFull)
                    {
                        MoveUnitFromIdleToBack(_idleAllies.First());
                        wasAnyUnitMoved = true;
                    }
                } while (wasAnyUnitMoved);

                // Move enemies between lines
                do
                {
                    wasAnyUnitMoved = false;
                    if (_backEnemies.Any() && !IsFrontEnemiesFull)
                    {
                        MoveUnitFromBackToFront(_backEnemies.First());
                        wasAnyUnitMoved = true;
                    }
                    if (_idleEnemies.Any() && !IsBackEnemiesFull)
                    {
                        MoveUnitFromIdleToBack(_idleEnemies.First());
                        wasAnyUnitMoved = true;
                    }
                } while (wasAnyUnitMoved);
                
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
            ui.gameObject.SetActive(true);
        }

        public void OnLoseFocus()
        {
            ui.gameObject.SetActive(false);
        }

        public Vector3 GetCameraPosition()
        {
            return cameraPosition.position;
        }

        public void StartEndlessFight()
        {
            StartEncounter(new EndlessFight(this, endlessFightData));
        }

        public void StartStressTest()
        {
            StartEncounter(new StressTestEncounter(this));
            for (int i = 0; i < 105; i++)
            {
                SpawnUnit(new TestUnit(
                    root: this,
                    maxHp: 100,
                    armor: 1,
                    damage: 10,
                    attackSpeed: 1,
                    side: Side.Allies
                    ));
            }
        }

        #endregion
    }
}