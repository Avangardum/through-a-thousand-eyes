using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.AdventureMapModule
{
    public class AdventureMapModuleFacade : IModuleFacade, IFocusable
    {
        public MainModuleFacade MainModuleFacade;
        private AdventureMapRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            _root = Object.FindObjectOfType<AdventureMapRoot>();
        }

        public JObject SaveModuleToJson()
        {
            return new JObject();
        }

        public string GetJsonPropertyName()
        {
            return "adventureMapModule";
        }

        public void Tick(float deltaTime)
        {
            
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