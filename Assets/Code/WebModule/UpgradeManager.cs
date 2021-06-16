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
            // TODO Check if has enough silk
            // TODO Remove silk
            FeedingGroundsLevel++;
            _root.SpawnCommonSpider();
        }
        
        // TODO add check if can upgrade
        // TODO add upgrade price field
    }
}