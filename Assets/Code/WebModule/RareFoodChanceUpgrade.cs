namespace ThroughAThousandEyes.WebModule
{
    public class RareFoodChanceUpgrade : RareOrShinyFoodChanceUpgrade
    {
        public RareFoodChanceUpgrade(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
        }

        public override string GetJsonTokenName()
        {
            return "rareFoodChanceUpgrade";
        }
    }
}