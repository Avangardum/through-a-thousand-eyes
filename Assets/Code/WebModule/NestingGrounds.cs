namespace ThroughAThousandEyes.WebModule
{
    public class NestingGrounds : Upgrade
    {
        public override string GetCurrentEffectText()
        {
            return $"+{Level} spiders";
        }

        public override void LevelUp()
        {
            base.LevelUp();
            _root.SpawnCommonSpider();
        }

        public override string GetJsonTokenName()
        {
            return "nestingGrounds";
        }

        public NestingGrounds(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
        }
    }
}