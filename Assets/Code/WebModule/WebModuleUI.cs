using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ThroughAThousandEyes.WebModule
{
    public class WebModuleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI upgradeName;
        [SerializeField] private TextMeshProUGUI upgradeDescription;
        [SerializeField] private TextMeshProUGUI upgradeCurrentLevel;
        [SerializeField] private TextMeshProUGUI upgradeCurrentEffect;
        [FormerlySerializedAs("upgradeButton")] [SerializeField] private Button levelUpButton;
        [FormerlySerializedAs("upgradeButtonText")] [SerializeField] private TextMeshProUGUI levelUpButtonText;
        [SerializeField] private Button nestingGroundsButton;

        private UpgradeManager _upgradeManager;
        private WebModuleRoot _root;
        private Action _levelUp;
        private Func<bool> _canAffordUpgrade;
        private bool _isInitialized;
        private Upgrade _currentUpgrade;
        
        private void ShowNestingGrounds()
        {
            ShowUpgrade(_upgradeManager.NestingGrounds);
        }

        private void ShowUpgrade(Upgrade upgrade)
        {
            _currentUpgrade = upgrade;
            upgradeName.text = upgrade.Name;
            upgradeDescription.text = upgrade.Description;
            upgradeCurrentLevel.text = $"Current Level: {upgrade.Level}";
            upgradeCurrentEffect.text = $"Current Effect: {upgrade.GetCurrentEffectText()}";
            levelUpButtonText.text = $"Level Up ({upgrade.GetNextUpgradePrice()})";
            _levelUp = _upgradeManager.LevelUpNestingGrounds;
            _canAffordUpgrade = () => _upgradeManager.CanAffordUpgrade(upgrade);
        }

        private void Update()
        {
            if (!_isInitialized)
                return;
            
            levelUpButton.interactable = _canAffordUpgrade();
        }

        private void LevelUp()
        {
            _levelUp();
            ShowUpgrade(_currentUpgrade);
        }

        public void Initialize(WebModuleRoot webModuleRoot)
        {
            _root = webModuleRoot;
            _upgradeManager = _root.UpgradeManager;
            levelUpButton.onClick.AddListener(LevelUp);
            nestingGroundsButton.onClick.AddListener(ShowNestingGrounds);
            
            ShowNestingGrounds();
            _isInitialized = true;
        }
    }
}