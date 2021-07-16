using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.AdventureMapModule;
using ThroughAThousandEyes.CheatsModule;
using ThroughAThousandEyes.CombatModule;
using ThroughAThousandEyes.GeneralUIModule;
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
        public ActivitySwitcher ActivitySwitcher { get; private set; }

        private List<IModuleFacade> _moduleFacades;
        private MainModuleRoot _root;
        
        // Module facades
        public MainModuleFacade MainModuleFacade;
        public WebModuleFacade WebModuleFacade;
        public GeneralUIModuleFacade GeneralUIModuleFacade;
        public CheatsModuleFacade CheatsModuleFacade;
        public CombatModuleFacade CombatModuleFacade;
        public AdventureMapModuleFacade AdventureMapModuleFacade;
        
        public void Initialize(MainModuleFacade facade, JObject saveData = null)
        {
            MainModuleFacade = facade;
            FocusManager.Initialize(this);
            ActivitySwitcher = new ActivitySwitcher(this);
            Inventory = new Inventory(this, saveData?[InventoryTokenName].ToObject<JObject>());
            MainSpiderStats = new MainSpiderStats(this, saveData?[MainSpiderStatsTokenName]?.ToObject<JObject>());
            SaveManager = new SaveManager(_moduleFacades);
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
        }

        public void InitializeGame(JObject saveData = null)
        {
            // Create module facades
            _moduleFacades = new List<IModuleFacade>();
            MainModuleFacade = new MainModuleFacade();
            _moduleFacades.Add(MainModuleFacade);
            WebModuleFacade = new WebModuleFacade();
            _moduleFacades.Add(WebModuleFacade);
            GeneralUIModuleFacade = new GeneralUIModuleFacade();
            _moduleFacades.Add(GeneralUIModuleFacade);
            CheatsModuleFacade = new CheatsModuleFacade();
            _moduleFacades.Add(CheatsModuleFacade);
            CombatModuleFacade = new CombatModuleFacade();
            _moduleFacades.Add(CombatModuleFacade);
            AdventureMapModuleFacade = new AdventureMapModuleFacade();
            _moduleFacades.Add(AdventureMapModuleFacade);

            // Initialize modules using facades
            foreach (var facade in _moduleFacades)
            {
                facade.InitializeModule(MainModuleFacade, saveData?[facade.GetJsonPropertyName()]?.ToObject<JObject>());
            }
        }
        
        private void FixedUpdate()
        {
            foreach (var facade in _moduleFacades)
            {
                facade.Tick(Time.fixedDeltaTime);
            }
        }

        private void Awake()
        {
            InitializeGame(SaveManager.SaveDataExists ? SaveManager.LoadSaveData() : null);
        }
    }
}