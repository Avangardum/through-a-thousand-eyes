namespace ThroughAThousandEyes.WebModule
{
    public abstract class Upgrade
    {
        public string Name;
        public string Description;
        public int Level = 0;
        public IntegerProgressionData PriceProgression;

        public long GetNextUpgradePrice() => PriceProgression.GetElement(Level + 1);
        public abstract string GetCurrentEffectText();

        public virtual void Initialize(WebModuleData.UpgradeData data)
        {
            Name = data.Name;
            Description = data.Description;
            PriceProgression = data.Price;
        }

        public Upgrade(WebModuleData.UpgradeData data)
        {
            Name = data.Name;
            Description = data.Description;
            PriceProgression = data.Price;
        }
    }
}