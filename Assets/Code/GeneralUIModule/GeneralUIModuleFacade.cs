using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.GeneralUIModule
{
    public class GeneralUIModuleFacade : IModuleFacade
    {
        private MainModuleFacade _mainModuleFacade;
        private GeneralUIRoot _root;

        public Inventory Inventory => _mainModuleFacade.Inventory;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _mainModuleFacade = mainModuleFacade;
            _root = UnityEngine.Object.FindObjectOfType<GeneralUIRoot>();
            _root.Initialize(this, saveData);
        }

        public JObject SaveModuleToJson()
        {
            return new JObject();
        }

        public string GetJsonPropertyName()
        {
            return "generalUiModule";
        }

        public void Tick(float deltaTime)
        {
            
        }
    }
}