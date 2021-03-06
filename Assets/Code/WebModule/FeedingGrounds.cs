using System;

namespace ThroughAThousandEyes.WebModule
{
    public class FeedingGrounds : Upgrade
    {
        public double DamageBonus => Level * _damageBonusPerLevel;

        public double DamageMultiplier => DamageBonus + 1;
        
        private double _damageBonusPerLevel;

        public FeedingGrounds(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
            _damageBonusPerLevel = ((WebModuleData.FeedingGroundsData) data).DamageIncreasePercentagePerLevel;
        }

        public override string GetCurrentEffectText()
        {
            return $"+{Math.Round(DamageBonus * 100, 2)}% damage";
        }

        public override string GetJsonTokenName()
        {
            return "feedingGrounds";
        }
    }
}