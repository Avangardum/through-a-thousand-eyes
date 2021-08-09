using System;
using ThroughAThousandEyes.MainModule;

namespace ThroughAThousandEyes.CombatModule
{
    public class MainSpider : Unit
    {
        private MainSpiderStats MainSpiderStats => _root.Facade.MainModuleFacade.MainSpiderStats;
        
        public override double CurrentHp
        {
            get => MainSpiderStats.CurrentHp;
            protected set => MainSpiderStats.CurrentHp = value;
        }
        
        public override double MaxHp
        {
            get => MainSpiderStats.MaxHp;
        }

        public override double Armor
        {
            get => MainSpiderStats.Armor;
        }

        public override double Damage
        {
            get => MainSpiderStats.Damage;
        }

        public override double AttackSpeed
        {
            get => MainSpiderStats.AttackSpeed;
        }

        public MainSpider(CombatModuleRoot root) : base(root)
        {
            
        }

        public override void GiveExp(double amount)
        {
            TteLogger.WriteMessage($"Main spider received {amount} exp in fight");
            _root.Facade.MainModuleFacade.MainSpiderStats.Experience += amount;
        }
    }
}