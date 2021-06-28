using System;

namespace ThroughAThousandEyes.WebModule
{
    public class UpgradeManager
    {
        private WebModuleRoot _root;

        public readonly NestingGrounds NestingGrounds;
        private long SilkInInventory => _root.Facade.Inventory.Silk;

        public UpgradeManager(WebModuleRoot root) // Initialize here
        {
            _root = root;

            NestingGrounds = new NestingGrounds(_root.Data.NestingGrounds);
        }

        public void LevelUpNestingGrounds()
        {
            SpendSilk(NestingGrounds.GetNextUpgradePrice());
            NestingGrounds.Level++;
            _root.SpawnCommonSpider();
        }

        public void SpendSilk(long amount)
        {
            if (SilkInInventory < amount)
            {
                throw new Exception("Not enough silk");
            }

            _root.Facade.Inventory.Silk -= amount;
        }

        public bool CanAffordUpgrade(Upgrade upgrade) => SilkInInventory >= upgrade.GetNextUpgradePrice();
    }
}