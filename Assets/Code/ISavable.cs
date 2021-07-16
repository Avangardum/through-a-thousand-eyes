using Newtonsoft.Json.Linq;

namespace ThroughAThousandEyes
{
    public interface ISavable
    {
        JObject SaveModuleToJson();
        string GetJsonPropertyName();
    }
}