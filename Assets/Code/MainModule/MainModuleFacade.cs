using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.CheatsModule;
using ThroughAThousandEyes.CombatModule;
using ThroughAThousandEyes.GeneralUIModule;
using ThroughAThousandEyes.WebModule;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThroughAThousandEyes.MainModule
{
    public class MainModuleFacade : IModuleFacade
    {
        private const string InventoryTokenName = "inventory";
        private const string MainSpiderStatsTokenName = "mainSpiderStats";
        
        public Inventory Inventory { get; private set; }
        public SaveManager SaveManager { get; private set; }
        public MainSpiderStats MainSpiderStats { get; private set; }

        private List<IModuleFacade> _moduleFacades;
        private MainModuleUnityInterface _unityInterface;
        private MainModuleRoot _root;
        
        // Module facades
        public WebModuleFacade _webModuleFacade;
        public GeneralUIModuleFacade _generalUIModuleFacade;
        public CheatsModuleFacade _cheatsModuleFacade;
        public CombatModuleFacade _combatModuleFacade;
        
        public ActivitySwitcher ActivitySwitcher => _root.ActivitySwitcher;

        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _root = Object.FindObjectOfType<MainModuleRoot>();
            _root.Initialize(this, saveData);
            _unityInterface = new GameObject("Main Module Unity Interface").AddComponent<MainModuleUnityInterface>();
            _unityInterface.EFixedUpdate += OnFixedUpdate;
            Inventory = new Inventory(mainModuleFacade, saveData?[InventoryTokenName].ToObject<JObject>());
            MainSpiderStats = new MainSpiderStats(mainModuleFacade, saveData?[MainSpiderStatsTokenName]?.ToObject<JObject>());
            SaveManager = new SaveManager(_moduleFacades);
        }

        public JObject SaveModuleToJson()
        {
            return new JObject(
                new JProperty(InventoryTokenName, Inventory.SaveToJson()),
                new JProperty(MainSpiderStatsTokenName, MainSpiderStats.SaveToJson())
                );
        }

        public string GetJsonPropertyName()
        {
            return "mainModule";
        }

        public void Tick(float deltaTime)
        {
            SaveManager.Tick(deltaTime);
        }

        public void InitializeGame(JObject saveData = null)
        {
            // Create module facades
            _moduleFacades = new List<IModuleFacade>();
            _moduleFacades.Add(this);
            _webModuleFacade = new WebModuleFacade();
            _moduleFacades.Add(_webModuleFacade);
            _generalUIModuleFacade = new GeneralUIModuleFacade();
            _moduleFacades.Add(_generalUIModuleFacade);
            _cheatsModuleFacade = new CheatsModuleFacade();
            _moduleFacades.Add(_cheatsModuleFacade);
            _combatModuleFacade = new CombatModuleFacade();
            _moduleFacades.Add(_combatModuleFacade);

            // Initialize modules using facades
            foreach (var facade in _moduleFacades)
            {
                facade.InitializeModule(this, saveData?[facade.GetJsonPropertyName()]?.ToObject<JObject>());
            }
        }
        
        private void OnFixedUpdate()
        {
            foreach (var facade in _moduleFacades)
            {
                facade.Tick(Time.fixedDeltaTime);
            }
        }
    }
}