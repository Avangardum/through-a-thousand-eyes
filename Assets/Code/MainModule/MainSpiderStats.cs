using System;
using Newtonsoft.Json.Linq;

namespace ThroughAThousandEyes.MainModule
{
    public class MainSpiderStats
    {
        private const string LevelTokenName = "level";
        private const string ExperienceTokenName = "experience";
        private const string CurrentHpTokenName = "currentHp";
        private const string MaxHpTokenName = "MaxHp";
        private const string ArmorTokenName = "armor";
        private const string DamageTokenName = "damage";
        private const string SpeedTokenName = "speed";
        private const string AttackSpeedTokenName = "attackSpeed";
        private const string SkillPointsTokenName = "skillPoints";

        private MainModuleRoot _root;
        
        private double _experience;

        private long _level = 1;
        public double CurrentHp = 100;
        public double MaxHp = 100;
        public double Armor = 1;
        public double Damage = 10;
        public double Speed = 1;
        public double AttackSpeed = 1;
        public long SkillPoints;

        public double Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                double expToLevelUp;
                while (true)
                {
                    expToLevelUp = _root.Data.ExperienceToGetLevelN.GetElement(Level + 1);
                    if (_experience >= expToLevelUp)
                    {
                        _experience -= expToLevelUp;
                        Level++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        public long Level
        {
            get => _level;
            set
            {
                var delta = value - _level;
                if (delta < 0)
                {
                    throw new InvalidOperationException("Can't decrease level");
                }
                _level = value;
                SkillPoints += delta;
            }
        }

        public JObject SaveToJson()
        {
            return new JObject(
                new JProperty(LevelTokenName, Level),
                new JProperty(ExperienceTokenName, Experience),
                new JProperty(CurrentHpTokenName, CurrentHp),
                new JProperty(MaxHpTokenName, MaxHp),
                new JProperty(ArmorTokenName, Armor),
                new JProperty(DamageTokenName, Damage),
                new JProperty(SpeedTokenName, Speed),
                new JProperty(AttackSpeedTokenName, AttackSpeed),
                new JProperty(SkillPointsTokenName, SkillPoints)
            );
        }

        public MainSpiderStats(MainModuleRoot root, JObject saveData = null)
        {
            _root = root;
            
            if (saveData != null)
            {
                Level = saveData[LevelTokenName]?.ToObject<long>() ?? Level;
                Experience = saveData[ExperienceTokenName]?.ToObject<double>() ?? Experience;
                CurrentHp = saveData[CurrentHpTokenName]?.ToObject<double>() ?? CurrentHp;
                MaxHp = saveData[MaxHpTokenName]?.ToObject<double>() ?? MaxHp;
                Armor = saveData[ArmorTokenName]?.ToObject<double>() ?? Armor;
                Damage = saveData[DamageTokenName]?.ToObject<double>() ?? Damage;
                Speed = saveData[SpeedTokenName]?.ToObject<double>() ?? Speed;
                AttackSpeed = saveData[AttackSpeedTokenName]?.ToObject<double>() ?? AttackSpeed;
                SkillPoints = saveData[SkillPointsTokenName]?.ToObject<long>() ?? SkillPoints;
            }
        }
    }
}
