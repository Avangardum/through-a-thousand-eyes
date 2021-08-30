using System;
using ThroughAThousandEyes.MainModule;
using TMPro;
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
        [SerializeField] private Button _kingdomAttackButton;
        [SerializeField] private Button _kingdomDefenceButton;
        [SerializeField] private TextMeshProUGUI _kingdomDefenceStagesPassedText;

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

        private void OnClickKingdomAttack()
        {
            
        }

        private void OnClickKingdomDefence()
        {
            _root.Facade.MainModuleFacade.ActivitySwitcher.StartKingdomDefence();
        }

        public void Initialize(AdventureMapModuleRoot root)
        {
            _root = root;
            webButton.onClick.AddListener(OnClickWebButton);
            endlessFightButton.onClick.AddListener(OnClickEndlessFightButton);
            combatStressTestButton.onClick.AddListener(OnClickCombatStressTestButton);
            mainSpiderUpgradingButton.onClick.AddListener(OnClickMainSpiderUpgradingButton);
            _kingdomAttackButton.onClick.AddListener(OnClickKingdomAttack);
            _kingdomDefenceButton.onClick.AddListener(OnClickKingdomDefence);
        }

        private void Update()
        {
            _kingdomDefenceStagesPassedText.text = $"Kingdom defence stages passed (weakens enemies in kingdom attack):" +
                                                   $" {_root.Facade.MainModuleFacade.KingdomDefenceStagesPassed}";
        }
    }
}