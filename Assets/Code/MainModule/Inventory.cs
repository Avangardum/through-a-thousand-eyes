using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class Inventory
    {
        private const string SilkTokenName = "silk";
        
        public long Silk;

        public JObject SaveToJson()
        {
            return new JObject(
                new JProperty(SilkTokenName, Silk)
            );
        }

        public Inventory(MainModuleFacade facade, JObject saveData = null)
        {
            if (saveData != null)
            {
                Silk = saveData[SilkTokenName].ToObject<long>();
            }
        }
    }
}
