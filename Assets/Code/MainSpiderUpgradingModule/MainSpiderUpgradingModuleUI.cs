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
            if (MainSpiderStats.SkillPoints <= 0)
                throw new Exception("No skill points");
            MainSpiderStats.SkillPoints--;
            MainSpiderStats.AttackSpeed += _root.Data.AttackSpeedPerSkillPoint;
        }

        private void UpgradeSpeed()
        {
            if (MainSpiderStats.SkillPoints <= 0)
                throw new Exception("No skill points");
            MainSpiderStats.SkillPoints--;
            MainSpiderStats.Speed += _root.Data.SpeedPerSkillPoint;
        }

        private void UpgradeDamage()
        {
            if (MainSpiderStats.SkillPoints <= 0)
                throw new Exception("No skill points");
            MainSpiderStats.SkillPoints--;
            MainSpiderStats.Damage += _root.Data.DamagePerSkillPoint;
        }

        private void UpgradeArmor()
        {
            if (MainSpiderStats.SkillPoints <= 0)
                throw new Exception("No skill points");
            MainSpiderStats.SkillPoints--;
            MainSpiderStats.Armor += _root.Data.ArmorPerSkillPoint;
        }

        private void UpgradeHP()
        {
            if (MainSpiderStats.SkillPoints <= 0)
                throw new Exception("No skill points");
            MainSpiderStats.SkillPoints--;
            MainSpiderStats.MaxHp += _root.Data.MaxHpPerSkillPoint;
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