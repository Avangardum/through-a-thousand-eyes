namespace ThroughAThousandEyes.CombatModule
{
    public class TestUnit : Unit
    {
        public TestUnit(CombatModuleRoot root, double maxHp, double armor, double damage, double attackSpeed, Side side)
            : base(root, maxHp, armor, damage, attackSpeed, side)
        {
        }
        
        public TestUnit(CombatModuleRoot root, double maxHp, double armor, double damage, double attackSpeed, Side side, long expReward)
            : base(root, maxHp, armor, damage, attackSpeed, side, expReward)
        {
        }
    }
}