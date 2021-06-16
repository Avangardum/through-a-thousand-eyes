using System;
using TMPro;
using UnityEngine;

namespace ThroughAThousandEyes.GeneralUIModule
{
    public class GeneralUIRoot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _silkCounterNumber;

        public GeneralUIModuleFacade Facade { get; private set; }

        private void Update()
        {
            _silkCounterNumber.text = Facade.Inventory.Silk.ToString();
        }

        public void Initialize(GeneralUIModuleFacade facade, bool isLoadingSavedGame, string saveData = "")
        {
            Facade = facade;
        }
    }
}