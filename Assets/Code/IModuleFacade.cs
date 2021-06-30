using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;

namespace ThroughAThousandEyes
{
    public interface IModuleFacade
    {
        void InitializeModule(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "");
        JObject SaveModuleToJson();
        string GetJsonPropertyName();
        void Tick(float deltaTime);
    }
}
