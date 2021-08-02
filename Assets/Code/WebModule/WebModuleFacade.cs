using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleFacade : IModuleFacade, IFocusable, ITickable, ISavable
    {
        private WebModuleRoot _root;
        public MainModuleFacade MainModuleFacade { get; private set; }

        public Inventory Inventory => MainModuleFacade.Inventory;
        public MainSpiderStats MainSpiderStats => MainModuleFacade.MainSpiderStats;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _root = Object.FindObjectOfType<WebModuleRoot>();
            _root.Initialize(this, saveData);
            MainModuleFacade = mainModuleFacade;
        }

        public JObject SaveModuleToJson()
        {
            return _root.SaveModuleToJson();
        }

        public string GetJsonPropertyName()
        {
            return "webModule";
        }

        public void Tick(float deltaTime)
        {
            _root.Tick(deltaTime);
        }

        public void OnGetFocus()
        {
            _root.OnGetFocus();
        }

        public void OnLoseFocus()
        {
            _root.OnLoseFocus();
        }

        public Vector3 GetCameraPosition()
        {
            return _root.GetCameraPosition();
        }
    }
}