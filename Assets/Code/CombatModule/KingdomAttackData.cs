using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    [CreateAssetMenu(menuName = "Kingdom Attack Data")]
    public class KingdomAttackData : ScriptableObject
    {
        [field: SerializeField] public int CommonWavesAmount { get; private set; }
        [field: SerializeField] public ProgressionData<int> KingdomWarriorsInCommonWaveByWaveNumber { get; private set; }
        [field: SerializeField] public UnitStats KingdomWarriorStats { get; private set; }
        [field: SerializeField] public UnitStats KingStats { get; set; }
        [field: SerializeField] public UnitStats KnightStats { get; private set; }
        [field: SerializeField] public float StatsDecreasePercentageForKingdomDefenceStage { get; private set; }
        [field: SerializeField] public float MaxStatsDecreasePercentage { get; private set; }
    }
}