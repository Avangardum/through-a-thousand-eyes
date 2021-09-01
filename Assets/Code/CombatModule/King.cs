using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class King : Unit
    {
        private readonly List<float> _summonKnightThresholds = new List<float> { 0.5f, 0.1f };
        
        private UnitStats _knightStats;
        
        public King(CombatModuleRoot root, UnitStats stats, UnitStats knightStats) : base(root, stats, Side.Enemies)
        {
            _knightStats = knightStats;
        }
        
        public override void ReceiveDamage(double damage, Unit source)
        {
            base.ReceiveDamage(damage, source);
            if (IsDead) return;

            while (_summonKnightThresholds.Any(x => x >= CurrentHpPercentage))
            {
                _summonKnightThresholds.Remove(_summonKnightThresholds.First(x => x >= CurrentHpPercentage));
                _root.SpawnUnit(new Knight(_root, _knightStats));
            }
        }
    }
}