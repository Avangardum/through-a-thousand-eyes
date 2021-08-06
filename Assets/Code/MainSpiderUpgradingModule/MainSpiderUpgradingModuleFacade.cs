using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.MainSpiderUpgradingModule
{
    public class MainSpiderUpgradingModuleFacade : IModuleFacade, IFocusable
    {
        public MainModuleFacade MainModuleFacade;
        private MainSpiderUpgradingModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            _root = Object.FindObjectOfType<MainSpiderUpgradingModuleRoot>();
            _root.Initialize(this);
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