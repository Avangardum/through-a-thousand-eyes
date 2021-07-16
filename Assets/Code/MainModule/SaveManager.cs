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
        private const float AutosaveInterval = 60;
        
        private readonly List<IModuleFacade> _moduleFacades;
        private float _timeUntilAutosave = AutosaveInterval;
        
        private static string FullPath => Application.persistentDataPath + '/' + SaveFileName;

        public static bool SaveDataExists => File.Exists(FullPath);
        
        public SaveManager(List<IModuleFacade> moduleFacades)
        {
            _moduleFacades = moduleFacades;
        }

        public void SaveGame()
        {
            JObject save = new JObject(
                _moduleFacades.Select(x => new JProperty(x.GetJsonPropertyName(), x.SaveModuleToJson()))
                );
            File.WriteAllText(FullPath, save.ToString());
        }

        public static JObject LoadSaveData()
        {
            return JObject.Parse(File.ReadAllText(FullPath));
        }

        public void Tick(float deltaTime)
        {
            _timeUntilAutosave -= deltaTime;
            if (_timeUntilAutosave <= 0)
            {
                SaveGame();
                _timeUntilAutosave = AutosaveInterval;
            }
        }
    }
}