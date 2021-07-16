using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleFacade : IModuleFacade, IFocusable, ITickable, ISavable
    {
        public MainModuleFacade MainModuleFacade;
        
        private CombatModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            _root = Object.FindObjectOfType<CombatModuleRoot>();
            _root.Initialize(this, saveData);
        }

        public JObject SaveModuleToJson()
        {
            return new JObject();
        }

        public string GetJsonPropertyName()
        {
            return "combatModule";
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

        public void StartEndlessFight() => _root.StartEndlessFight();
    }
}