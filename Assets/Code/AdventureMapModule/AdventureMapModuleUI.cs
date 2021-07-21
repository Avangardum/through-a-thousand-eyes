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

        public void Initialize(AdventureMapModuleRoot root)
        {
            _root = root;
            webButton.onClick.AddListener(OnClickWebButton);
            endlessFightButton.onClick.AddListener(OnClickEndlessFightButton);
            combatStressTestButton.onClick.AddListener(OnClickCombatStressTestButton);
        }
    }
}