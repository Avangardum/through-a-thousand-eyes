using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    [CreateAssetMenu(menuName = "Data/Main Module Data")]
    public class MainModuleData : ScriptableObject
    {
        [field: SerializeField] public ProgressionData<double> ExperienceToGetLevelN { get; private set; }
        [field: SerializeField] public int InitialSkillPointsPerLevel { get; private set; } = 5;
        [field: SerializeField] public int DecreaseSkillPointsPerLevelEveryNLevels { get; private set; } = 1000;
        [field: SerializeField] public float MainSpiderRegenPercentagePerSeconds { get; private set; } = 0.02f;
    }
}