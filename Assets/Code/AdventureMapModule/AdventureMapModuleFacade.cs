using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.AdventureMapModule
{
    public class AdventureMapModuleFacade : IModuleFacade, IFocusable
    {
        public MainModuleFacade MainModuleFacade;
        private AdventureMapModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            _root = Object.FindObjectOfType<AdventureMapModuleRoot>();
            _root.Initialize(this, saveData);
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