namespace ThroughAThousandEyes.WebModule
{
    public abstract class Upgrade
    {
        public string Name;
        public string Description;
        public int Level = 0;
        public IntegerProgressionData PriceProgression;

        public long GetNextUpgradePrice() => PriceProgression.GetElement(Level + 1);

        protected readonly WebModuleRoot _root;

        public abstract string GetCurrentEffectText();

        public virtual void LevelUp()
        {
            _root.UpgradeManager.SpendSilk(GetNextUpgradePrice());
            Level++;
        }
        
        public Upgrade(WebModuleData.UpgradeData data, WebModuleRoot root)
        {
            Name = data.Name;
            Description = data.Description;
            PriceProgression = data.Price;
            _root = root;
        }
    }
}