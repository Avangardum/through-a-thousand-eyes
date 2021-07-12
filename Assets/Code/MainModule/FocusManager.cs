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
        
        public void Initialize(GeneralUIModuleFacade generalUIModuleFacade, WebModuleFacade webModuleFacade, CombatModuleFacade combatModuleFacade)
        {
            _generalUIModuleFacade = generalUIModuleFacade;
            _webModuleFacade = webModuleFacade;
            _combatModuleFacade = combatModuleFacade;
            
            _focusableFacades = new IFocusable[] {_webModuleFacade, _combatModuleFacade};
        }

        public void FocusOnWeb() => FocusOnModule(_webModuleFacade, true);

        public void FocusOnCombat() => FocusOnModule(_combatModuleFacade,false);

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
            FocusOnWeb();
        }
    }
}