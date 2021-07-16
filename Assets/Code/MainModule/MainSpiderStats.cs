using Newtonsoft.Json.Linq;

namespace ThroughAThousandEyes.MainModule
{
    public class MainSpiderStats
    {
        private const string LevelTokenName = "level";
        private const string CurrentHpTokenName = "currentHp";
        private const string MaxHpTokenName = "MaxHp";
        private const string ArmorTokenName = "armor";
        private const string DamageTokenName = "damage";
        private const string SpeedTokenName = "speed";
        private const string AttackSpeedTokenName = "attackSpeed";
        
        public long Level = 1;
        public double CurrentHp = 100;
        public double MaxHp = 100;
        public double Armor = 1;
        public double Damage = 10;
        public double Speed = 1;
        public double AttackSpeed = 1;

        public JObject SaveToJson()
        {
            return new JObject(
                new JProperty(LevelTokenName, Level),
                new JProperty(CurrentHpTokenName, CurrentHp),
                new JProperty(MaxHpTokenName, MaxHp),
                new JProperty(ArmorTokenName, Armor),
                new JProperty(DamageTokenName, Damage),
                new JProperty(SpeedTokenName, Speed),
                new JProperty(AttackSpeedTokenName, AttackSpeed)
            );
        }

        public MainSpiderStats(MainModuleRoot root, JObject saveData = null)
        {
            if (saveData != null)
            {
                Level = saveData[LevelTokenName].ToObject<long>();
                CurrentHp = saveData[CurrentHpTokenName].ToObject<double>();
                MaxHp = saveData[MaxHpTokenName].ToObject<double>();
                Armor = saveData[ArmorTokenName].ToObject<double>();
                Damage = saveData[DamageTokenName].ToObject<double>();
                Speed = saveData[SpeedTokenName].ToObject<double>();
                AttackSpeed = saveData[AttackSpeedTokenName].ToObject<double>();
            }
        }
    }
}
