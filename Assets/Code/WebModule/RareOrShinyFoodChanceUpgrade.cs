using System;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public abstract class RareOrShinyFoodChanceUpgrade : Upgrade
    {
        public float Chance => _chancePerLevel * Level;
        
        private float _chancePerLevel;
        
        public RareOrShinyFoodChanceUpgrade(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
            _chancePerLevel = ((WebModuleData.RareOrShinyFoodSpawnChanceUpgradeData) data).ChancePerLevel;
        }

        public override string GetCurrentEffectText()
        {
            return $"{Mathf.Floor(Chance)} guaranteed, {Math.Round((Chance - Mathf.Floor(Chance)) * 100, 2)}% chance of an extra one";
        }
    }
}