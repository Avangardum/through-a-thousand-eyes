using UnityEngine;

namespace ThroughAThousandEyes.MainSpiderUpgradingModule
{
    [CreateAssetMenu(menuName = "Data/Main Spider Upgrading Data")]
    public class MainSpiderUpgradingData : ScriptableObject
    {
        [field: SerializeField] public double MaxHpPerSkillPoint { get; private set; } = 1;
        [field: SerializeField] public double DamagePerSkillPoint { get; private set; } = 1;
        [field: SerializeField] public double ArmorPerSkillPoint { get; private set; } = 1;
        [field: SerializeField] public double SpeedPerSkillPoint { get; private set; } = 1;
        [field: SerializeField] public double AttackSpeedPerSkillPoint { get; private set; } = 1;
    }
}