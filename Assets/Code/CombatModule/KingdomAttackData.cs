using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    [CreateAssetMenu(menuName = "Data/Kingdom Attack Data")]
    public class KingdomAttackData : ScriptableObject
    {
        [field: SerializeField] public int CommonWavesAmount { get; private set; }
        [field: SerializeField] public ProgressionData<int> KingdomWarriorsInCommonWaveByWaveNumber { get; private set; }
        [field: SerializeField] public UnitStats KingdomWarriorStats { get; private set; }
        [field: SerializeField] public UnitStats KingStats { get; set; }
        [field: SerializeField] public UnitStats KnightStats { get; private set; }
        [field: SerializeField] public float StatsDecreasePercentageForKingdomDefenceStage { get; private set; }
        [field: SerializeField] public float MaxStatsDecreasePercentage { get; private set; }

        public UnitStats KingdomWarriorStatsAfterDecrease(int defenceStagesPassed) =>
            MultiplyUnitStats(KingdomWarriorStats, StatsMultiplier(defenceStagesPassed));
        
        public UnitStats KingStatsAfterDecrease(int defenceStagesPassed) =>
            MultiplyUnitStats(KingStats, StatsMultiplier(defenceStagesPassed));
        
        public UnitStats KnightStatsAfterDecrease(int defenceStagesPassed) =>
            MultiplyUnitStats(KnightStats, StatsMultiplier(defenceStagesPassed));

        private UnitStats MultiplyUnitStats(UnitStats baseStats, float multiplier)
        {
            baseStats.Armor *= multiplier;
            baseStats.Damage *= multiplier;
            baseStats.AttackSpeed *= multiplier;
            baseStats.MaxHp *= multiplier;
            return baseStats;
        }

        private float StatsMultiplier(int defenceStagesPassed)
        {
            return Mathf.Min(StatsDecreasePercentageForKingdomDefenceStage * defenceStagesPassed, MaxStatsDecreasePercentage);
        }
    }
}