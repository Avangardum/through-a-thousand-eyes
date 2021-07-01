using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace ThroughAThousandEyes.WebModule
{
    public class UpgradeManager
    {
        private const string LevelJsonTokenPostfix = "Level";
        
        private WebModuleRoot _root;

        public readonly NestingGrounds NestingGrounds;
        public readonly AcidicWeb AcidicWeb;
        public readonly FeedingGrounds FeedingGrounds;
        public readonly FattyInsects FattyInsects;
        public readonly CollectiveFeeding CollectiveFeeding;
        public readonly StickierWeb StickierWeb;
        public readonly FoodSpawnChanceUpgrade FoodSpawnChanceUpgrade;
        public readonly RareOrShinyFoodChanceUpgrade RareFoodSpawnChangeUpgrade;
        public readonly RareOrShinyFoodChanceUpgrade ShinyFoodSpawnChangeUpgrade;
        private long SilkInInventory => _root.SilkInInventory;
        private List<Upgrade> _upgrades = new List<Upgrade>();

        public UpgradeManager(WebModuleRoot root, JObject saveData) // Initialize here
        {
            _root = root;

            NestingGrounds = new NestingGrounds(_root.Data.NestingGrounds, _root);
            _upgrades.Add(NestingGrounds);
            AcidicWeb = new AcidicWeb(_root.Data.AcidicWeb, _root);
            _upgrades.Add(AcidicWeb);
            FeedingGrounds = new FeedingGrounds(_root.Data.FeedingGrounds, _root);
            _upgrades.Add(FeedingGrounds);
            FattyInsects = new FattyInsects(_root.Data.FattyInsects, _root);
            _upgrades.Add(FattyInsects);
            CollectiveFeeding = new CollectiveFeeding(_root.Data.CollectiveFeeding, _root);
            _upgrades.Add(CollectiveFeeding);
            StickierWeb = new StickierWeb(_root.Data.StickierWeb, _root);
            _upgrades.Add(StickierWeb);
            FoodSpawnChanceUpgrade = new FoodSpawnChanceUpgrade(_root.Data.FoodSpawnChanceUpgrade, _root);
            _upgrades.Add(FoodSpawnChanceUpgrade);
            RareFoodSpawnChangeUpgrade = new RareFoodChanceUpgrade(_root.Data.RareFoodSpawnChanceUpgrade, _root);
            _upgrades.Add(RareFoodSpawnChangeUpgrade);
            ShinyFoodSpawnChangeUpgrade = new ShinyFoodChanceUpgrade(_root.Data.ShinyFoodSpawnChanceUpgrade, _root);
            _upgrades.Add(ShinyFoodSpawnChangeUpgrade);

            if (saveData != null)
            {
                foreach (var upgrade in _upgrades)
                {
                    if (saveData[upgrade.GetJsonTokenName() + LevelJsonTokenPostfix] != null)
                    {
                        upgrade.Level = saveData[upgrade.GetJsonTokenName() + LevelJsonTokenPostfix].ToObject<int>();
                    }
                }
            }
        }
        
        public void SpendSilk(long amount) => _root.SpendSilk(amount);
        
        public bool CanAffordUpgrade(Upgrade upgrade) => SilkInInventory >= upgrade.GetNextUpgradePrice();

        public JObject SaveToJson()
        {
            return new JObject(_upgrades.Select(x => new JProperty(x.GetJsonTokenName() + LevelJsonTokenPostfix, x.Level)));
        }
    }
}