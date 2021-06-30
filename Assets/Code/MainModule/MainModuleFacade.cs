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
        public Inventory Inventory { get; private set; }
        public MainSpiderStats MainSpiderStats { get; private set; }

        private List<IModuleFacade> _moduleFacades;
        private MainModuleUnityInterface _unityInterface;
        
        // Module facades
        private WebModuleFacade _webModuleFacade;
        private GeneralUIModuleFacade _generalUIModuleFacade;
        private CheatsModuleFacade _cheatsModuleFacade;

        public void InitializeModule(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _unityInterface = new GameObject("Main Module Unity Interface").AddComponent<MainModuleUnityInterface>();
            _unityInterface.EFixedUpdate += OnFixedUpdate;
            Inventory = new Inventory();
            MainSpiderStats = new MainSpiderStats();
        }

        public JObject SaveModuleToJson()
        {
            return new JObject();
        }

        public string GetJsonPropertyName()
        {
            return "mainModule";
        }

        public void Tick(float deltaTime)
        {
            
        }

        public void InitializeGame(bool isLoadingSavedGame, string saveData = "")
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
                facade.InitializeModule(this, isLoadingSavedGame, saveData);
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