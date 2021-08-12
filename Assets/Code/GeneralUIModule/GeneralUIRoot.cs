using System;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughAThousandEyes.GeneralUIModule
{
    public class GeneralUIRoot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _silkCounterNumber;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _mapButton;

        public GeneralUIModuleFacade Facade { get; private set; }

        private void Update()
        {
            _silkCounterNumber.text = Facade.Inventory.Silk.ToString();
        }

        public void Initialize(GeneralUIModuleFacade facade, JObject saveData = null)
        {
            Facade = facade;
            _saveButton.onClick.AddListener(Save);
            _mapButton.onClick.AddListener(GoToMap);
        }

        private void Save()
        {
            Facade.MainModuleFacade.SaveManager.SaveGame();
        }

        private void GoToMap()
        {
            Facade.MainModuleFacade.ActivitySwitcher.SwitchToAdventureMap();
        }
    }
}