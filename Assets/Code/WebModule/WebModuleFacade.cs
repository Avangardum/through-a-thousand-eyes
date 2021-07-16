using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleFacade : IModuleFacade, IFocusable, ITickable, ISavable
    {
        private WebModuleRoot _root;
        private MainModuleFacade _mainModuleFacade;

        public Inventory Inventory => _mainModuleFacade.Inventory;
        public MainSpiderStats MainSpiderStats => _mainModuleFacade.MainSpiderStats;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _root = Object.FindObjectOfType<WebModuleRoot>();
            _root.Initialize(this, saveData);
            _mainModuleFacade = mainModuleFacade;
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