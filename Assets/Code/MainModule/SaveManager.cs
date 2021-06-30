using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class SaveManager
    {
        private const string SaveFileName = "save.txt";
        
        private List<IModuleFacade> _moduleFacades;
        
        private static string fullPath => Application.persistentDataPath + '/' + SaveFileName;

        public static bool SaveDataExists => File.Exists(fullPath);
        
        public SaveManager(List<IModuleFacade> moduleFacades)
        {
            _moduleFacades = moduleFacades;
        }

        public void SaveGame()
        {
            JObject save = new JObject(
                _moduleFacades.Select(x => new JProperty(x.GetJsonPropertyName(), x.SaveModuleToJson()))
                );
            File.WriteAllText(fullPath, save.ToString());
        }

        public static JObject LoadSaveData()
        {
            return JObject.Parse(File.ReadAllText(fullPath));
        }
    }
}