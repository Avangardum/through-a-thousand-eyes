using System;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    [CreateAssetMenu(menuName = "Data/Web Module Data", fileName = "Web Module Data")]
    public class WebModuleData : ScriptableObject
    {
        [field: SerializeField] public float WebWidth { get; private set; } = 7;
        [field: SerializeField] public float WebHeight { get; private set; } = 7;
        [field: SerializeField] public float FoodWaveInterval { get; private set; } = 1;
        [field: SerializeField] public float AttackRange { get; private set; } = 0.5f;
        [field: SerializeField] public float AttackInterval { get; private set; } = 1f;
        [field: SerializeField] public double ExperienceToLevelUpBase { get; private set; } = 100;
        [field: SerializeField] public double ExperienceToLevelUpAddition { get; private set; } = 10;

        [field: Header("Upgrades")]
        
        [field: SerializeField] public NestingGroundsData NestingGrounds { get; private set; }
        [field: SerializeField] public AcidicWebData AcidicWeb { get; private set; }
        [field: SerializeField] public FeedingGroundsData FeedingGrounds { get; private set; }
        [field: SerializeField] public FattyInsectsData FattyInsects { get; private set; }
        [field: SerializeField] public CollectiveFeedingData CollectiveFeeding { get; private set; }
        [field: SerializeField] public StickierWebData StickierWeb { get; private set; }

    [Serializable]
        public class UpgradeData
        {
            [field: SerializeField] public IntegerProgressionData Price { get; private set; }
        }
        
        [Serializable]
        public class NestingGroundsData : UpgradeData
        {
            [field: SerializeField] public int SpidersPerLevel { get; private set; } = 1;
        }
        
        [Serializable]
        public class AcidicWebData : UpgradeData
        {
            [field: SerializeField] public double DpsPerLevel { get; private set; }
        }
        
        [Serializable]
        public class FeedingGroundsData : UpgradeData
        {
            [field: SerializeField] public double DamageIncreasePercentagePerLevel { get; private set; }
        }
        
        [Serializable]
        public class FattyInsectsData : UpgradeData
        {
            [field: SerializeField] public double HpIncreasePercentagePerLevel { get; private set; }
        }
        
        [Serializable]
        public class CollectiveFeedingData : UpgradeData
        {
            [field: SerializeField] public double DamageIncreasePercentagePerSpiderPerLevel { get; private set; }
        }
        
        [Serializable]
        public class StickierWebData : UpgradeData
        {
            [field: SerializeField] public double ExtraTimePerLevel { get; private set; }
        }
    }
}