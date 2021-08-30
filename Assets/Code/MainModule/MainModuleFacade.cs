using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.AdventureMapModule;
using ThroughAThousandEyes.CheatsModule;
using ThroughAThousandEyes.CombatModule;
using ThroughAThousandEyes.GeneralUIModule;
using ThroughAThousandEyes.WebModule;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThroughAThousandEyes.MainModule
{
    public class MainModuleFacade : IModuleFacade, ISavable, ITickable
    {
        public Inventory Inventory => _root.Inventory;
        public MainSpiderStats MainSpiderStats => _root.MainSpiderStats;
        public ActivitySwitcher ActivitySwitcher => _root.ActivitySwitcher;
        public SaveManager SaveManager => _root.SaveManager;
        public MainModuleData Data => _root.Data;
        public int KingdomDefenceStagesPassed => _root.CombatModuleFacade.KingdomDefenceStagePassed;

        private MainModuleRoot _root;

        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _root = Object.FindObjectOfType<MainModuleRoot>();
            _root.Initialize(this, saveData);

        }

        public JObject SaveModuleToJson()
        {
            return _root.SaveModuleToJson();
        }

        public string GetJsonPropertyName()
        {
            return "mainModule";
        }

        public void Tick(float deltaTime)
        {
            _root.Tick(deltaTime);
        }
    }
}