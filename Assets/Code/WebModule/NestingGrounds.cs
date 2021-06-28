namespace ThroughAThousandEyes.WebModule
{
    public class NestingGrounds : Upgrade
    {
        public override string GetCurrentEffectText()
        {
            return $"+{Level} spiders";
        }

        public NestingGrounds(WebModuleData.UpgradeData data) : base(data)
        {
        }
    }
}