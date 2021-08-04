using System;
using ThroughAThousandEyes.MainModule;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughAThousandEyes.MainSpiderUpgradingModule
{
    public class MainSpiderUpgradingModuleUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI MaxHpText;
        [SerializeField] private TextMeshProUGUI ArmorText;
        [SerializeField] private TextMeshProUGUI DamageText;
        [SerializeField] private TextMeshProUGUI SpeedText;
        [SerializeField] private TextMeshProUGUI AttackSpeedText;
        [SerializeField] private TextMeshProUGUI SkillPointsText;
        [SerializeField] private Button MaxHPButton;
        [SerializeField] private Button ArmorButton;
        [SerializeField] private Button DamageButton;
        [SerializeField] private Button SpeedButton;
        [SerializeField] private Button AttackSpeedButton;
        
        private MainSpiderUpgradingModuleRoot _root;
        private Button[] _buttons;
        private bool _isInitialized;
        
        private MainSpiderStats MainSpiderStats => _root.Facade.MainModuleFacade.MainSpiderStats;

        public void Initialize(MainSpiderUpgradingModuleRoot root)
        {
            _root = root;
            _buttons = new[] {MaxHPButton, ArmorButton, DamageButton, SpeedButton, AttackSpeedButton};
            MaxHPButton.onClick.AddListener(UpgradeHP);
            ArmorButton.onClick.AddListener(UpgradeArmor);
            DamageButton.onClick.AddListener(UpgradeDamage);
            SpeedButton.onClick.AddListener(UpgradeSpeed);
            AttackSpeedButton.onClick.AddListener(UpgradeAttackSpeed);

            _isInitialized = true;
        }

        private void UpgradeAttackSpeed()
        {
            throw new NotImplementedException();
        }

        private void UpgradeSpeed()
        {
            throw new NotImplementedException();
        }

        private void UpgradeDamage()
        {
            throw new NotImplementedException();
        }

        private void UpgradeArmor()
        {
            throw new NotImplementedException();
        }

        private void UpgradeHP()
        {
            throw new NotImplementedException();
        }

        private void Update()
        {
            if (!_isInitialized)
                return;

            MaxHpText.text = $"Max HP: {MainSpiderStats.MaxHp}";
            ArmorText.text = $"Armor: {MainSpiderStats.Armor}";
            DamageText.text = $"Damage: {MainSpiderStats.Damage}";
            SpeedText.text = $"Speed: {MainSpiderStats.Speed}";
            AttackSpeedText.text = $"Attack Speed: {MainSpiderStats.AttackSpeed}";
            SkillPointsText.text = $"Skill Points: {MainSpiderStats.SkillPoints}";
            
            foreach (var button in _buttons)
            {
                button.interactable = MainSpiderStats.SkillPoints > 0;
            }
        }
    }
}