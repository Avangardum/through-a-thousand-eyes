using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.GeneralUIModule
{
    public class GeneralUIModuleFacade : IModuleFacade
    {
        public MainModuleFacade MainModuleFacade { get; private set; }
        private GeneralUIRoot _root;

        public Inventory Inventory => MainModuleFacade.Inventory;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            _root = UnityEngine.Object.FindObjectOfType<GeneralUIRoot>();
            _root.Initialize(this, saveData);
        }
        
        public void SetActive(bool isActive) => _root.gameObject.SetActive(isActive);
    }
}