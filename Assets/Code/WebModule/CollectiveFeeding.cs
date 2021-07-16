using System;

namespace ThroughAThousandEyes.WebModule
{
    public class CollectiveFeeding : Upgrade
    {
        public double DamageIncreasePerSpider => Level * _damageIncreasePerSpiderPerLevel;

        private double _damageIncreasePerSpiderPerLevel;
        
        public CollectiveFeeding(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
            _damageIncreasePerSpiderPerLevel =
                ((WebModuleData.CollectiveFeedingData) data).DamageIncreasePercentagePerSpiderPerLevel;
        }

        public override string GetCurrentEffectText()
        {
            return $"+{Math.Round(DamageIncreasePerSpider * 100, 2)}% damage per spider";
        }

        public override string GetJsonTokenName()
        {
            return "collectiveFeeding";
        }
    }
}