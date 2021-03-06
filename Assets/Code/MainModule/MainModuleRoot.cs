using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.AdventureMapModule;
using ThroughAThousandEyes.CheatsModule;
using ThroughAThousandEyes.CombatModule;
using ThroughAThousandEyes.GeneralUIModule;
using ThroughAThousandEyes.MainSpiderUpgradingModule;
using ThroughAThousandEyes.WebModule;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class MainModuleRoot : MonoBehaviour
    {
        private const string InventoryTokenName = "inventory";
        private const string MainSpiderStatsTokenName = "mainSpiderStats";
        
        public Inventory Inventory { get; private set; }
        public SaveManager SaveManager { get; private set; }
        public MainSpiderStats MainSpiderStats { get; private set; }
        [field: SerializeField] public FocusManager FocusManager { get; private set; }
        [field: SerializeField] public MainModuleData Data { get; private set; }
        [field: SerializeField] public SaveMethod SaveMethod { get; private set; }
        public ActivitySwitcher ActivitySwitcher { get; private set; }

        public List<IModuleFacade> ModuleFacades;
        private List<ISavable> _savableFacades;
        private List<ITickable> _tickableFacades;
        private List<IStartable> _startableFacades;
        private MainModuleRoot _root;
        
        // Module facades
        public MainModuleFacade MainModuleFacade;
        public WebModuleFacade WebModuleFacade;
        public GeneralUIModuleFacade GeneralUIModuleFacade;
        public CheatsModuleFacade CheatsModuleFacade;
        public CombatModuleFacade CombatModuleFacade;
        public AdventureMapModuleFacade AdventureMapModuleFacade;
        public MainSpiderUpgradingModuleFacade MainSpiderUpgradingModuleFacade;
        
        public void Initialize(MainModuleFacade facade, JObject saveData = null)
        {
            MainModuleFacade = facade;
            FocusManager.Initialize(this);
            ActivitySwitcher = new ActivitySwitcher(this);
            Inventory = new Inventory(this, saveData?[InventoryTokenName].ToObject<JObject>());
            MainSpiderStats = new MainSpiderStats(this, saveData?[MainSpiderStatsTokenName]?.ToObject<JObject>());
            SaveManager = new SaveManager(_savableFacades, this);
        }
        
        public JObject SaveModuleToJson()
        {
            return new JObject(
                new JProperty(InventoryTokenName, Inventory.SaveToJson()),
                new JProperty(MainSpiderStatsTokenName, MainSpiderStats.SaveToJson())
            );
        }
        
        public void Tick(float deltaTime)
        {
            SaveManager.Tick(deltaTime);
            MainSpiderStats.Tick(deltaTime);
        }

        public void InitializeGame(JObject saveData = null)
        {
            TteLogger.WriteMessage(saveData == null ? "Starting new game" : "Loading saved game");
            
            // Create module facades
            ModuleFacades = new List<IModuleFacade>();
            MainModuleFacade = new MainModuleFacade();
            ModuleFacades.Add(MainModuleFacade);
            WebModuleFacade = new WebModuleFacade();
            ModuleFacades.Add(WebModuleFacade);
            GeneralUIModuleFacade = new GeneralUIModuleFacade();
            ModuleFacades.Add(GeneralUIModuleFacade);
            CheatsModuleFacade = new CheatsModuleFacade();
            ModuleFacades.Add(CheatsModuleFacade);
            CombatModuleFacade = new CombatModuleFacade();
            ModuleFacades.Add(CombatModuleFacade);
            AdventureMapModuleFacade = new AdventureMapModuleFacade();
            ModuleFacades.Add(AdventureMapModuleFacade);
            MainSpiderUpgradingModuleFacade = new MainSpiderUpgradingModuleFacade();
            ModuleFacades.Add(MainSpiderUpgradingModuleFacade);

            _savableFacades = ModuleFacades.Select(x => x as ISavable).Where(x => x != null).ToList();
            _tickableFacades = ModuleFacades.Select(x => x as ITickable).Where(x => x != null).ToList();
            _startableFacades = ModuleFacades.Select(x => x as IStartable).Where(x => x != null).ToList();
            
            // Initialize modules using facades
            foreach (var facade in ModuleFacades)
            {
                JObject sd = facade is ISavable savableFacade
                    ? saveData?[savableFacade.GetJsonPropertyName()]?.ToObject<JObject>()
                    : null;
                facade.InitializeModule(MainModuleFacade, sd);
            }
            
            TteLogger.WriteMessage("Initialization finished");
        }
        
        private void FixedUpdate()
        {
            foreach (var facade in _tickableFacades)
            {
                facade.Tick(Time.fixedDeltaTime);
            }
        }

        private void Awake()
        {
            InitializeGame(SaveManager.SaveDataExists ? SaveManager.LoadSaveData() : null);
        }

        private void Start()
        {
            foreach (var facade in _startableFacades)
            {
                facade.Start();
            }
        }
    }
}