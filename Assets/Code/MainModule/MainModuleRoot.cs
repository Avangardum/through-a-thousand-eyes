using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class MainModuleRoot : MonoBehaviour
    {
        [SerializeField] private FocusManager focusManager;

        public MainModuleFacade Facade;
        
        public void Initialize(MainModuleFacade facade, JObject saveData = null)
        {
            Facade = facade;
            focusManager.Initialize(facade._generalUIModuleFacade ,facade._webModuleFacade, facade._combatModuleFacade);
        }
    }
}