using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleFacade : IModuleFacade
    {
        private WebModuleRoot _root;
        private MainModuleFacade _mainModuleFacade;

        public Inventory Inventory => _mainModuleFacade.Inventory;
        public MainSpiderStats MainSpiderStats => _mainModuleFacade.MainSpiderStats;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _root = Object.FindObjectOfType<WebModuleRoot>();
            _root.Initialize(this, isLoadingSavedGame, saveData);
            _mainModuleFacade = mainModuleFacade;
        }

        public string SaveModule()
        {
            throw new System.NotImplementedException();
        }

        public void Tick(float deltaTime)
        {
            
        }
    }
}