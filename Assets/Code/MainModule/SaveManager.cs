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
        
        public SaveManager(List<IModuleFacade> moduleFacades)
        {
            _moduleFacades = moduleFacades;
        }

        public void SaveGame()
        {
            JObject save = new JObject(
                _moduleFacades.Select(x => new JProperty(x.GetJsonPropertyName(), x.SaveModuleToJson()))
                );
            string fullPath = Application.persistentDataPath + '/' + SaveFileName;
            Debug.Log(fullPath);
            File.WriteAllText(fullPath, save.ToString());
        }
    }
}