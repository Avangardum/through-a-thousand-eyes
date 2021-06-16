using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.GeneralUIModule
{
    public class GeneralUIModuleFacade : IModuleFacade
    {
        private MainModuleFacade _mainModuleFacade;
        private GeneralUIRoot _root;

        public Inventory Inventory => _mainModuleFacade.Inventory;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _mainModuleFacade = mainModuleFacade;
            _root = UnityEngine.Object.FindObjectOfType<GeneralUIRoot>();
            _root.Initialize(this, isLoadingSavedGame, saveData);
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