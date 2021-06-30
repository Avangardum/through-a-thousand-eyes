using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;

namespace ThroughAThousandEyes
{
    public interface IModuleFacade
    {
        void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null);
        JObject SaveModuleToJson();
        string GetJsonPropertyName();
        void Tick(float deltaTime);
    }
}
