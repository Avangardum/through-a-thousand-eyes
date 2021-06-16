using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    [CreateAssetMenu(menuName = "Data/Web Module Data", fileName = "Web Module Data")]
    public class WebModuleData : ScriptableObject
    {
        [field: SerializeField] public float WebWidth { get; private set; } = 7;
        [field: SerializeField] public float WebHeight { get; private set; } = 7;
        [field: SerializeField] public float FoodWaveInterval { get; private set; } = 1;
        [field: SerializeField] public float AttackRange { get; private set; } = 0.5f;
        [field: SerializeField] public float AttackInterval { get; private set; } = 1f;
        [field: SerializeField] public decimal ExperienceToLevelUpBase { get; private set; } = 100;
        [field: SerializeField] public decimal ExperienceToLevelUpAddition { get; private set; } = 10;
    }
}