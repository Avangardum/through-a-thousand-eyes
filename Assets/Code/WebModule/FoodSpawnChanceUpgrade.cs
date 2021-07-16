using System;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class FoodSpawnChanceUpgrade : Upgrade
    {
        public float ChanceBonus => Level * _chanceBonusPerLevel;

        public float TotalChance => _root.Data.BaseFoodSpawnChance + ChanceBonus;

        private float _chanceBonusPerLevel;

        public FoodSpawnChanceUpgrade(WebModuleData.UpgradeData data, WebModuleRoot root) : base(data, root)
        {
            _chanceBonusPerLevel = ((WebModuleData.FoodSpawnChanceUpgradeData) data).ChanceIncreasePerLevel;
        }

        public override string GetCurrentEffectText()
        {
            return $"{Mathf.Floor(TotalChance)} guaranteed, {Math.Round((TotalChance - Mathf.Floor(TotalChance)) * 100, 2)}% chance of an extra one";
        }

        public override void LevelUp()
        {
            base.LevelUp();
        }

        public override string GetJsonTokenName()
        {
            return "foodSpawnChanceUpgrade";
        }
    }
}