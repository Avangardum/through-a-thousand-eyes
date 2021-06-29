using System;

namespace ThroughAThousandEyes.WebModule
{
    public class UpgradeManager
    {
        private WebModuleRoot _root;

        public readonly NestingGrounds NestingGrounds;
        public readonly AcidicWeb AcidicWeb;
        public readonly FeedingGrounds FeedingGrounds;
        public readonly FattyInsects FattyInsects;
        public readonly CollectiveFeeding CollectiveFeeding;
        public readonly StickierWeb StickierWeb;
        private long SilkInInventory => _root.SilkInInventory;

        public UpgradeManager(WebModuleRoot root) // Initialize here
        {
            _root = root;

            NestingGrounds = new NestingGrounds(_root.Data.NestingGrounds, _root);
            AcidicWeb = new AcidicWeb(_root.Data.AcidicWeb, _root);
            FeedingGrounds = new FeedingGrounds(_root.Data.FeedingGrounds, _root);
            FattyInsects = new FattyInsects(_root.Data.FattyInsects, _root);
            CollectiveFeeding = new CollectiveFeeding(_root.Data.CollectiveFeeding, _root);
            StickierWeb = new StickierWeb(_root.Data.StickierWeb, _root);
        }
        
        public void SpendSilk(long amount) => _root.SpendSilk(amount);
        
        public bool CanAffordUpgrade(Upgrade upgrade) => SilkInInventory >= upgrade.GetNextUpgradePrice();
    }
}