using System;
using System.Collections.Generic;
using ThroughAThousandEyes.MainModule;
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
        [SerializeField] private Button levelUpButton;
        [SerializeField] private TextMeshProUGUI levelUpButtonText;
        [SerializeField] private Button nestingGroundsButton;
        [SerializeField] private Button acidicWebButton;
        [SerializeField] private Button feedingGroundsButton;
        [SerializeField] private Button fattyInsectsButton;
        [SerializeField] private Button collectiveFeedingButton;
        [SerializeField] private Button stickierWebButton;
        [SerializeField] private Button foodChanceUpgradeButton;
        [SerializeField] private Button rareFoodChanceUpgradeButton;
        [SerializeField] private Button shinyFoodChanceUpgradeButton;
        [SerializeField] private GameObject _webUpgradesPanel;
        [SerializeField] private GameObject _spidersPanel;
        [SerializeField] private GameObject _mainSpiderPanel;
        [SerializeField] private Button _webUpgradesButton;
        [SerializeField] private Button _spidersButton;
        [SerializeField] private Button _mainSpiderButton;
        [SerializeField] private TextMeshProUGUI _mainSpiderLevelText;
        [SerializeField] private TextMeshProUGUI _mainSpiderExpText;
        [SerializeField] private TextMeshProUGUI _mainSpiderSkillPointsText;
        [SerializeField] private SpidersSubpanel _spidersSubpanelPrefab;
        [SerializeField] private Transform _spidersSubpanelParent;

        private UpgradeManager _upgradeManager;
        private WebModuleRoot _root;
        private Action _levelUp;
        private Func<bool> _canAffordUpgrade;
        private bool _isInitialized;
        private Upgrade _currentUpgrade;
        private List<SpidersSubpanel> _spidersSubpanels = new List<SpidersSubpanel>();

        private MainSpiderStats MainSpiderStats => _root.Facade.MainSpiderStats;
        
        private void ShowNestingGrounds() => ShowUpgrade(_upgradeManager.NestingGrounds);
        private void ShowAcidicWeb() => ShowUpgrade(_upgradeManager.AcidicWeb);
        private void ShowFeedingGrounds() => ShowUpgrade(_upgradeManager.FeedingGrounds);
        private void ShowFattyInsects() => ShowUpgrade(_upgradeManager.FattyInsects);
        private void ShowCollectiveFeeding() => ShowUpgrade(_upgradeManager.CollectiveFeeding);
        private void ShowStickierWeb() => ShowUpgrade(_upgradeManager.StickierWeb);
        private void ShowFoodChanceUpgrade() => ShowUpgrade(_upgradeManager.FoodSpawnChanceUpgrade);
        private void ShowRareFoodChanceUpgrade() => ShowUpgrade(_upgradeManager.RareFoodSpawnChangeUpgrade);
        private void ShowShinyFoodChanceUpgrade() => ShowUpgrade(_upgradeManager.ShinyFoodSpawnChangeUpgrade);

        private void ShowUpgrade(Upgrade upgrade)
        {
            _currentUpgrade = upgrade;
            upgradeName.text = upgrade.Name;
            upgradeDescription.text = upgrade.Description;
            upgradeCurrentLevel.text = $"Current Level: {upgrade.Level}";
            upgradeCurrentEffect.text = $"Current Effect: {upgrade.GetCurrentEffectText()}";
            levelUpButtonText.text = $"Level Up ({upgrade.GetNextUpgradePrice()})";
            _levelUp = upgrade.LevelUp;
            _canAffordUpgrade = () => _upgradeManager.CanAffordUpgrade(upgrade);
        }

        private void Update()
        {
            if (!_isInitialized)
                return;

            if (_webUpgradesPanel.activeInHierarchy)
            {
                levelUpButton.interactable = _canAffordUpgrade();
            }

            if (_mainSpiderPanel.activeInHierarchy)
            {
                _mainSpiderLevelText.text = $"Level\n{MainSpiderStats.Level}";
                _mainSpiderExpText.text = $"Experience\n{MainSpiderStats.Experience:F0}/{MainSpiderStats.ExperienceToLevelUp:F0}";
                _mainSpiderSkillPointsText.text = $"Skill Points\n{MainSpiderStats.SkillPoints}";
            }

            if (_spidersPanel.activeInHierarchy)
            {
                // Create lacking subpanels
                for (int i = _spidersSubpanels.Count; i < _root.Spiders.Count; i++)
                {
                    var spiderSubpanel = Instantiate(_spidersSubpanelPrefab, _spidersSubpanelParent);
                    spiderSubpanel.Name = _root.Spiders[i].IsMainSpider ? "Main spider" : "Common spider";
                    spiderSubpanel.Spider = _root.Spiders[i];
                    _spidersSubpanels.Add(spiderSubpanel);
                }
                // Update panels data
                for (int i = 0; i < _spidersSubpanels.Count; i++)
                {
                    _spidersSubpanels[i].Level = _root.Spiders[i].Level;
                }
            }
        }

        private void LevelUp()
        {
            _levelUp();
            ShowUpgrade(_currentUpgrade);
        }

        public void Initialize(WebModuleRoot webModuleRoot)
        {
            InitializeUpgrades(webModuleRoot);
            InitializeSpidersPanel(webModuleRoot);
            _isInitialized = true;
        }

        private void InitializeSpidersPanel(WebModuleRoot webModuleRoot)
        {
            
        }

        private void InitializeUpgrades(WebModuleRoot webModuleRoot)
        {
            _root = webModuleRoot;
            _upgradeManager = _root.UpgradeManager;
            levelUpButton.onClick.AddListener(LevelUp);
            nestingGroundsButton.onClick.AddListener(ShowNestingGrounds);
            acidicWebButton.onClick.AddListener(ShowAcidicWeb);
            feedingGroundsButton.onClick.AddListener(ShowFeedingGrounds);
            fattyInsectsButton.onClick.AddListener(ShowFattyInsects);
            collectiveFeedingButton.onClick.AddListener(ShowCollectiveFeeding);
            stickierWebButton.onClick.AddListener(ShowStickierWeb);
            foodChanceUpgradeButton.onClick.AddListener(ShowFoodChanceUpgrade);
            rareFoodChanceUpgradeButton.onClick.AddListener(ShowRareFoodChanceUpgrade);
            shinyFoodChanceUpgradeButton.onClick.AddListener(ShowShinyFoodChanceUpgrade);
            _webUpgradesButton.onClick.AddListener(ShowWebUpgradesPanel);
            _spidersButton.onClick.AddListener(ShowSpidersPanel);
            _mainSpiderButton.onClick.AddListener(ShowMainSpiderPanel);
            ShowNestingGrounds();
        }

        private void ShowMainSpiderPanel()
        {
            _mainSpiderPanel.SetActive(true);
            _spidersPanel.SetActive(false);
            _webUpgradesPanel.SetActive(false);
        }

        private void ShowSpidersPanel()
        {
            _mainSpiderPanel.SetActive(false);
            _spidersPanel.SetActive(true);
            _webUpgradesPanel.SetActive(false);
        }

        private void ShowWebUpgradesPanel()
        {
            _mainSpiderPanel.SetActive(false);
            _spidersPanel.SetActive(false);
            _webUpgradesPanel.SetActive(true);
        }
    }
}