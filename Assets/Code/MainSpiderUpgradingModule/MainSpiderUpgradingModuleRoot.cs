using UnityEngine;

namespace ThroughAThousandEyes.MainSpiderUpgradingModule
{
    public class MainSpiderUpgradingModuleRoot : MonoBehaviour, IFocusable
    {
        public MainSpiderUpgradingModuleFacade Facade;
        
        [SerializeField] private Transform _cameraPosition;
        [SerializeField] private MainSpiderUpgradingModuleUI _ui;
        
        public void OnGetFocus()
        {
            _ui.gameObject.SetActive(true);
        }

        public void OnLoseFocus()
        {
            _ui.gameObject.SetActive(false);
        }

        public Vector3 GetCameraPosition()
        {
            return _cameraPosition.position;
        }

        public void Initialize(MainSpiderUpgradingModuleFacade facade)
        {
            Facade = facade;
            _ui.Initialize(this);
        }
    }
}