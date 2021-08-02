using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    [CreateAssetMenu(menuName = "Data/Main Module Data")]
    public class MainModuleData : ScriptableObject
    {
        [field: SerializeField] public ProgressionData<double> ExperienceToGetLevelN { get; private set; }
    }
}