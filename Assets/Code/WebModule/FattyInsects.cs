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
            return $"+{HpBonus * 100}% health";
        }
    }
}