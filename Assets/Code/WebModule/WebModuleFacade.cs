using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleFacade : IModuleFacade
    {
        private WebModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _root = Object.FindObjectOfType<WebModuleRoot>();
            _root.Initialize(mainModuleFacade, isLoadingSavedGame, saveData);
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