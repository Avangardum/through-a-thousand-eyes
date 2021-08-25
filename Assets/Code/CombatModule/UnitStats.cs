using System;

namespace ThroughAThousandEyes.CombatModule
{
    [Serializable]
    public struct UnitStats
    {
        public double MaxHp;
        public double Armor;
        public double Damage;
        public double AttackSpeed;
        public long ExpReward;
    }
}