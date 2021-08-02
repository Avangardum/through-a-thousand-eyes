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
        [field: SerializeField] public double NormalFoodBaseHp { get; private set; } = 10;
        [field: SerializeField] public double BigFoodBaseHp { get; private set; } = 100;
        [field: SerializeField] public float NormalFoodBaseEscapeTime { get; private set; } = 15;
        [field: SerializeField] public float BigFoodBaseEscapeTime { get; private set; } = 20;
        [field: SerializeField] public float BigFoodBaseChance { get; private set; } = 0.1f;
        [field: SerializeField] public float BaseFoodSpawnChance { get; private set; } = 0.5f;
        [field: SerializeField] public float RareFoodExpMultiplier { get; private set; } = 3;
        [field: SerializeField] public float ShinyFoodExpMultiplier { get; private set; } = 10;

        [field: Header("Upgrades")]
        
        [field: SerializeField] public NestingGroundsData NestingGrounds { get; private set; }
        [field: SerializeField] public AcidicWebData AcidicWeb { get; private set; }
        [field: SerializeField] public FeedingGroundsData FeedingGrounds { get; private set; }
        [field: SerializeField] public FattyInsectsData FattyInsects { get; private set; }
        [field: SerializeField] public CollectiveFeedingData CollectiveFeeding { get; private set; }
        [field: SerializeField] public StickierWebData StickierWeb { get; private set; }
        [field: SerializeField] public FoodSpawnChanceUpgradeData FoodSpawnChanceUpgrade { get; private set; }
        [field: SerializeField] public RareOrShinyFoodSpawnChanceUpgradeData RareFoodSpawnChanceUpgrade { get; private set; }
        [field: SerializeField] public RareOrShinyFoodSpawnChanceUpgradeData ShinyFoodSpawnChanceUpgrade { get; private set; }
        
        [Serializable]
        public class UpgradeData
        {
            [field: SerializeField] public string Name { get; private set; }
            [field: TextArea(10, 20)] [field: SerializeField] public string Description { get; private set; }
            [field: SerializeField] public ProgressionData<long> Price { get; private set; }
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
            [field: SerializeField] public float ExtraTimePerLevel { get; private set; }
        }
        
        [Serializable]
        public class FoodSpawnChanceUpgradeData : UpgradeData
        {
            [field: SerializeField] public float ChanceIncreasePerLevel { get; private set; }
        }

        [Serializable]
        public class RareOrShinyFoodSpawnChanceUpgradeData : UpgradeData
        {
            [field: SerializeField] public float ChancePerLevel { get; private set; }
        }
    }
}