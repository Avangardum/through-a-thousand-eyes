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
        private bool _isInitializing;
        
        private double _experience;

        private long _level = 1;
        public double CurrentHp = 100;
        public double MaxHp = 100;
        public double Armor = 1;
        public double Damage = 10;
        public double Speed = 1;
        public double AttackSpeed = 1;
        public long SkillPoints;

        public double ExperienceToLevelUp => _root.Data.ExperienceToGetLevelN.GetElement(Level + 1);
        
        public double Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                if (_isInitializing)
                    return;
                
                while (true)
                {
                    if (_experience >= ExperienceToLevelUp)
                    {
                        _experience -= ExperienceToLevelUp;
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
                if (_isInitializing)
                {
                    _level = value;
                    return;
                }
                
                var delta = value - _level;
                if (delta < 0)
                {
                    throw new InvalidOperationException("Can't decrease level");
                }

                while (_level < value)
                {
                    _level++;
                    OnLevelUp(_level);
                }
            }
        }

        private void OnLevelUp(long newLevel)
        {
            long skillPointsDelta = Math.Max(
                _root.Data.InitialSkillPointsPerLevel - newLevel / _root.Data.DecreaseSkillPointsPerLevelEveryNLevels, 
                1
                );
            TteLogger.WriteMessage($"Main spider leveled up to level {newLevel} and gained {skillPointsDelta} skill points");
            SkillPoints += skillPointsDelta;
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
            _isInitializing = true;
            
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

            _isInitializing = false;
        }

        public void Tick(float deltaTime)
        {
            if (!_root.CombatModuleFacade.IsCombatActive)
            {
                CurrentHp = Math.Min(CurrentHp + _root.Data.MainSpiderRegenPercentagePerSeconds * deltaTime * MaxHp, MaxHp);
            }
        }
    }
}
