using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThroughAThousandEyes.MainModule
{
    public class SaveManager
    {
        private const string SaveFileName = "Save.txt";
        private const string SaveDataPlayerPrefsKey = "SaveData";
        private const float AutosaveInterval = 60;
        
        private readonly List<ISavable> _savableFacades;
        private float _timeUntilAutosave = AutosaveInterval;
        private readonly MainModuleRoot _root;

        private static string FullPath => Application.persistentDataPath + '/' + SaveFileName;
        public bool CanSave => !_root.CombatModuleFacade.IsCombatActive;
        
        public static bool SaveDataExists
        {
            get
            {
                switch (Object.FindObjectOfType<MainModuleRoot>().SaveMethod)
                {
                    case SaveMethod.File:
                        return File.Exists(FullPath);
                    case SaveMethod.PlayerPrefs:
                        return PlayerPrefs.HasKey(SaveDataPlayerPrefsKey);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        public SaveManager(List<ISavable> savableFacades, MainModuleRoot root)
        {
            _savableFacades = savableFacades;
            _root = root;
        }

        public void SaveGame()
        {
            if (!CanSave)
            {
                throw new Exception("Saving failed. Can't save now");
            }
            
            JObject save = new JObject(
                _savableFacades.Select(x => new JProperty(x.GetJsonPropertyName(), x.SaveModuleToJson()))
                );
            switch (_root.SaveMethod)
            {
                case SaveMethod.File:
                    File.WriteAllText(FullPath, save.ToString());
                    break;
                case SaveMethod.PlayerPrefs:
                    PlayerPrefs.SetString(SaveDataPlayerPrefsKey, save.ToString());
                    PlayerPrefs.Save();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static JObject LoadSaveData()
        {
            switch (Object.FindObjectOfType<MainModuleRoot>().SaveMethod)
            {
                case SaveMethod.File:
                    return JObject.Parse(File.ReadAllText(FullPath));
                case SaveMethod.PlayerPrefs:
                    return JObject.Parse(PlayerPrefs.GetString(SaveDataPlayerPrefsKey));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Tick(float deltaTime)
        {
            _timeUntilAutosave -= deltaTime;
            if (_timeUntilAutosave <= 0 && CanSave)
            {
                SaveGame();
                _timeUntilAutosave = AutosaveInterval;
            }
        }
    }
}