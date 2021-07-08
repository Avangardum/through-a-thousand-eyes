using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleFacade : IModuleFacade
    {
        public MainModuleFacade MainModuleFacade;
        
        private CombatModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            _root = UnityEngine.Object.FindObjectOfType<CombatModuleRoot>();
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
            _root.Tick();
        }
    }
}