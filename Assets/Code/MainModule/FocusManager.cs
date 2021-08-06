using System;
using System.Linq;
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
        private MainModuleRoot _root;
        
        public void Initialize(MainModuleRoot root)
        {
            _root = root;

            _focusableFacades = _root.ModuleFacades.Select(x => x as IFocusable).Where(x => x != null).ToArray();
        }

        public void FocusOnWeb() => FocusOnModule(_root.WebModuleFacade, true);

        public void FocusOnCombat() => FocusOnModule(_root.CombatModuleFacade,false);
        public void FocusOnAdventureMap() => FocusOnModule(_root.AdventureMapModuleFacade, true);
        public void FocusOnMainSpiderUpgrading() => FocusOnModule(_root.MainSpiderUpgradingModuleFacade, true);

        private void FocusOnModule(IFocusable module, bool showGeneralUI)
        {
            RemoveFocus();
            module.OnGetFocus();
            mainCamera.transform.position = module.GetCameraPosition();
            _root.GeneralUIModuleFacade.SetActive(showGeneralUI);
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