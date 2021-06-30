using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.CheatsModule;
using ThroughAThousandEyes.GeneralUIModule;
using ThroughAThousandEyes.WebModule;
using UnityEngine;

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
        
        // Module facades
        private WebModuleFacade _webModuleFacade;
        private GeneralUIModuleFacade _generalUIModuleFacade;
        private CheatsModuleFacade _cheatsModuleFacade;

        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _unityInterface = new GameObject("Main Module Unity Interface").AddComponent<MainModuleUnityInterface>();
            _unityInterface.EFixedUpdate += OnFixedUpdate;
            Inventory = new Inventory(mainModuleFacade, saveData?[InventoryTokenName].ToObject<JObject>());
            MainSpiderStats = new MainSpiderStats(mainModuleFacade, saveData?[MainSpiderStatsTokenName].ToObject<JObject>());
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
            
        }

        public void InitializeGame(JObject saveData = null)
        {
            // Create modules facades
            _moduleFacades = new List<IModuleFacade>();
            _moduleFacades.Add(this);
            _webModuleFacade = new WebModuleFacade();
            _moduleFacades.Add(_webModuleFacade);
            _generalUIModuleFacade = new GeneralUIModuleFacade();
            _moduleFacades.Add(_generalUIModuleFacade);
            _cheatsModuleFacade = new CheatsModuleFacade();
            _moduleFacades.Add(_cheatsModuleFacade);

            // Initialize modules using facades
            foreach (var facade in _moduleFacades)
            {
                facade.InitializeModule(this, saveData?[facade.GetJsonPropertyName()].ToObject<JObject>());
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