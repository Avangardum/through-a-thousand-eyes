namespace ThroughAThousandEyes.CombatModule
{
    /// <summary>
    /// Enemy from the endless fight. Any stats can be assigned 
    /// </summary>
    public class EndlessFightEnemy : Unit
    {
        public EndlessFightEnemy(CombatModuleRoot root, double maxHp, double armor, double damage, double attackSpeed) : 
            base(root, maxHp, armor, damage, attackSpeed, Side.Enemies)
        {
            
        }
    }
}