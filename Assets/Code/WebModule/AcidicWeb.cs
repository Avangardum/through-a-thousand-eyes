namespace ThroughAThousandEyes.WebModule
{
    public class AcidicWeb : Upgrade
    {
        public double DamagePerSecond => Level * _damagePerSecondPerLevel;

        private double _damagePerSecondPerLevel;
        
        public AcidicWeb(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
            _damagePerSecondPerLevel = ((WebModuleData.AcidicWebData) data).DpsPerLevel;
        }

        public override string GetCurrentEffectText()
        {
            return $"{DamagePerSecond} damage per second";
        }

        public override string GetJsonTokenName()
        {
            return "acidicWeb";
        }
    }
}