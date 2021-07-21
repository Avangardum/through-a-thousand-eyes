using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    [CreateAssetMenu(menuName = "Data/Endless Fight Data", fileName = "Endless Fight Data")]
    public class EndlessFightData : ScriptableObject
    {
        [field: SerializeField] public ProgressionData<double> EnemyHp { get; private set; }
        [field: SerializeField] public ProgressionData<double> EnemyArmor { get; private set; }
        [field: SerializeField] public ProgressionData<double> EnemyDamage { get; private set; }
        [field: SerializeField] public ProgressionData<double> EnemyAttackSpeed { get; private set; }
        [field: SerializeField] public ProgressionData<long> EnemyExpReward { get; private set; }
    }
}