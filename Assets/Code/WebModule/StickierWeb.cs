namespace ThroughAThousandEyes.WebModule
{
    public class StickierWeb : Upgrade
    {
        public float ExtraTime => Level * _extraTimePerLevel;

        private float _extraTimePerLevel;
        
        public StickierWeb(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
            _extraTimePerLevel = ((WebModuleData.StickierWebData) data).ExtraTimePerLevel;
        }

        public override string GetCurrentEffectText()
        {
            return $"+{ExtraTime} seconds";
        }
    }
}