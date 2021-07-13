using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class MainModuleRoot : MonoBehaviour
    {
        public FocusManager focusManager;
        public ActivitySwitcher ActivitySwitcher;

        public MainModuleFacade Facade;
        
        public void Initialize(MainModuleFacade facade, JObject saveData = null)
        {
            Facade = facade;
            focusManager.Initialize(facade._generalUIModuleFacade ,facade._webModuleFacade, facade._combatModuleFacade);
            ActivitySwitcher = new ActivitySwitcher(this);
        }
    }
}