using System;

namespace ThroughAThousandEyes.WebModule
{
    public class FattyInsects : Upgrade
    {
        public double HpBonus => Level * _hpBonusPerLevel;

        public double HpMultiplier => HpBonus + 1;
        
        private double _hpBonusPerLevel;
        
        public FattyInsects(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
            _hpBonusPerLevel = ((WebModuleData.FattyInsectsData) data).HpIncreasePercentagePerLevel;
        }

        public override string GetCurrentEffectText()
        {
            return $"+{Math.Round(HpBonus * 100, 2)}% health";
        }

        public override string GetJsonTokenName()
        {
            return "fattyInsects";
        }
    }
}