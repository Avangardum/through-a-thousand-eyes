namespace ThroughAThousandEyes.WebModule
{
    public class ShinyFoodChanceUpgrade : RareOrShinyFoodChanceUpgrade
    {
        public ShinyFoodChanceUpgrade(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
        }

        public override string GetJsonTokenName()
        {
            return "shinyFoodChanceUpgrade";
        }
    }
}