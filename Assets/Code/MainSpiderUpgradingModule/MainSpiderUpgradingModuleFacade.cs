using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.MainSpiderUpgradingModule
{
    public class MainSpiderUpgradingModuleFacade : IModuleFacade, IFocusable
    {
        private MainModuleFacade _mainModuleFacade;
        private MainSpiderUpgradingModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _mainModuleFacade = mainModuleFacade;
        }

        public void OnGetFocus()
        {
            throw new System.NotImplementedException();
        }

        public void OnLoseFocus()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetCameraPosition()
        {
            return _root.GetCameraPosition();
        }
    }
}