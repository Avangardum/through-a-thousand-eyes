using System.Collections.Generic;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleRoot : MonoBehaviour
    {
        [SerializeField] private GameObject mainSpiderPrefab;
        [SerializeField] private GameObject commonSpiderPrefab;

        private readonly List<Spider> _spiders = new List<Spider>();
        private Spider _mainSpider;

        public void Initialize(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _mainSpider = Instantiate(mainSpiderPrefab, transform).GetComponent<Spider>();
            _spiders.Add(_mainSpider);
        }
    }
}