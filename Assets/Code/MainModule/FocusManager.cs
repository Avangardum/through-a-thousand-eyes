using System;
using ThroughAThousandEyes.CombatModule;
using ThroughAThousandEyes.GeneralUIModule;
using ThroughAThousandEyes.WebModule;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class FocusManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainCamera;
        
        private IFocusable[] _focusableFacades;
        private GeneralUIModuleFacade _generalUIModuleFacade;
        private WebModuleFacade _webModuleFacade;
        private CombatModuleFacade _combatModuleFacade;
        private MainModuleRoot _root;
        
        public void Initialize(GeneralUIModuleFacade generalUIModuleFacade, WebModuleFacade webModuleFacade, CombatModuleFacade combatModuleFacade, MainModuleRoot root)
        {
            _root = root;
            _generalUIModuleFacade = generalUIModuleFacade;
            _webModuleFacade = webModuleFacade;
            _combatModuleFacade = combatModuleFacade;
            
            _focusableFacades = new IFocusable[] {_webModuleFacade, _combatModuleFacade, _root.Facade._adventureMapModuleFacade};
        }

        public void FocusOnWeb() => FocusOnModule(_webModuleFacade, true);

        public void FocusOnCombat() => FocusOnModule(_combatModuleFacade,false);
        public void FocusOnAdventureMap() => FocusOnModule(_root.Facade._adventureMapModuleFacade, true);

        private void FocusOnModule(IFocusable module, bool showGeneralUI)
        {
            RemoveFocus();
            module.OnGetFocus();
            mainCamera.transform.position = module.GetCameraPosition();
            _generalUIModuleFacade.SetActive(showGeneralUI);
        }

        private void RemoveFocus()
        {
            foreach (IFocusable facade in _focusableFacades)
            {
                facade.OnLoseFocus();
            }
        }

        private void Start()
        {
            FocusOnAdventureMap();
        }
    }
}