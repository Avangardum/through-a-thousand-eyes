namespace ThroughAThousandEyes.WebModule
{
    public class UpgradeManager
    {
        public int FeedingGroundsLevel { get; private set; }
        
        private WebModuleRoot _root;

        public UpgradeManager(WebModuleRoot root)
        {
            _root = root;
        }

        public void UpgradeFeedingGrounds()
        {
            FeedingGroundsLevel++;
            _root.SpawnCommonSpider();
        }
    }
}