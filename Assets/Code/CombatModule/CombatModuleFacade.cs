using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleFacade : IModuleFacade
    {
        private MainModuleFacade _mainModuleFacade;
        private CombatModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            _mainModuleFacade = mainModuleFacade;
            _root = UnityEngine.Object.FindObjectOfType<CombatModuleRoot>();
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
            
        }
    }
}