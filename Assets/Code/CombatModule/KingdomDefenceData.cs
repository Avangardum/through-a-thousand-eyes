using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    [CreateAssetMenu(menuName = "Data/Kingdom Defence Data")]
    public class KingdomDefenceData : ScriptableObject
    {
        [field: SerializeField] public ProgressionData<int> EnemyAmount { get; private set; }
        [field: SerializeField] public UnitStats KingdomWarriorStats { get; private set; }
    }
}