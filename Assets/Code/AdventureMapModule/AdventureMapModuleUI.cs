using ThroughAThousandEyes.MainModule;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughAThousandEyes.AdventureMapModule
{
    public class AdventureMapModuleUI : MonoBehaviour
    {
        [SerializeField] private Button webButton;
        [SerializeField] private Button endlessFightButton;
        [SerializeField] private Button combatStressTestButton;
        [SerializeField] private Button mainSpiderUpgradingButton;

        private AdventureMapModuleRoot _root;
        
        private ActivitySwitcher ActivitySwitcher => _root.Facade.MainModuleFacade.ActivitySwitcher;
        
        private void OnClickWebButton()
        {
            ActivitySwitcher.SwitchToWeb();
        }

        private void OnClickEndlessFightButton()
        {
            ActivitySwitcher.StartEndlessFight();
        }

        private void OnClickCombatStressTestButton()
        {
            ActivitySwitcher.StartStressTestFight();
        }

        private void OnClickMainSpiderUpgradingButton()
        {
            ActivitySwitcher.SwitchToMainSpiderUpgrading();
        }

        public void Initialize(AdventureMapModuleRoot root)
        {
            _root = root;
            webButton.onClick.AddListener(OnClickWebButton);
            endlessFightButton.onClick.AddListener(OnClickEndlessFightButton);
            combatStressTestButton.onClick.AddListener(OnClickCombatStressTestButton);
            mainSpiderUpgradingButton.onClick.AddListener(OnClickMainSpiderUpgradingButton);
        }
    }
}