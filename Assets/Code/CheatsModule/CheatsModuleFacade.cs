using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;

namespace ThroughAThousandEyes.CheatsModule
{
    public class CheatsModuleFacade : IModuleFacade
    {
        /// <summary>
        /// Don't use outside the cheats module!
        /// </summary>
        public static CheatsModuleFacade Instance;

        public MainModuleFacade MainModuleFacade { get; private set; }

        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            Instance = this;
        }
    }
}