using System;

namespace ThroughAThousandEyes.WebModule
{
    public class UpgradeManager
    {
        private WebModuleRoot _root;

        public readonly NestingGrounds NestingGrounds;
        public readonly AcidicWeb AcidicWeb;
        private long SilkInInventory => _root.SilkInInventory;

        public UpgradeManager(WebModuleRoot root) // Initialize here
        {
            _root = root;

            NestingGrounds = new NestingGrounds(_root.Data.NestingGrounds, root);
            AcidicWeb = new AcidicWeb(_root.Data.AcidicWeb, root);
        }
        
        public void SpendSilk(long amount) => _root.SpendSilk(amount);
        
        public bool CanAffordUpgrade(Upgrade upgrade) => SilkInInventory >= upgrade.GetNextUpgradePrice();
    }
}