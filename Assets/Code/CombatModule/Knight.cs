using System.Collections.Generic;

namespace ThroughAThousandEyes.CombatModule
{
    public class Knight : Unit
    {
        public Knight(CombatModuleRoot root, UnitStats stats) : base(root, stats, Side.Enemies)
        {
        }

        public override List<CombatSkill> CombatSkills => new List<CombatSkill> {CombatSkill.Provocation};
    }
}