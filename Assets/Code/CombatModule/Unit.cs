using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ThroughAThousandEyes.CombatModule
{
    public abstract class Unit
    {
        public event Action<Unit> Death;
        
        public virtual double CurrentHp { get; protected set; }
        public virtual double MaxHp { get; }
        public virtual double Armor { get; }
        public virtual double Damage { get; }
        public virtual double AttackSpeed { get; }
        public Side Side { get; set; }
        public UnitView View { get; set; }
        public Line Line { get; set; }
        public long ExpReward { get; private set; }
        protected readonly CombatModuleRoot _root;
        public double TimeUntilAttack { get; private set; }
        private bool _wasStartCalled;
        private bool _isDead;

        private double AttackInterval => 1 / AttackSpeed;
        protected virtual string Name => GetType().ToString().Split('.').Last();

        protected Unit(CombatModuleRoot root, double maxHp, double armor, double damage, double attackSpeed, Side side)
        {
            _root = root;
            MaxHp = maxHp;
            CurrentHp = MaxHp;
            Armor = armor;
            Damage = damage;
            AttackSpeed = attackSpeed;
            Side = side;
        }

        protected Unit(CombatModuleRoot root, double maxHp, double armor, double damage, double attackSpeed, Side side, long expReward)
        : this(root, maxHp, armor, damage, attackSpeed, side)
        {
            ExpReward = expReward;
        }

        protected Unit(CombatModuleRoot root, UnitStats stats, Side side) 
            : this(root, stats.MaxHp, stats.Armor, stats.Damage, stats.AttackSpeed, side, stats.ExpReward)
        {
            
        }

        /// <summary>
        /// Constructor for the main spider
        /// </summary>
        protected Unit(CombatModuleRoot root)
        {
            _root = root;
            Side = Side.Allies;
        }
        
        public virtual void Tick(float deltaTime)
        {
            if (_isDead)
            {
                return;
            }
            
            if (!_wasStartCalled)
            {
                Start();
            }

            TimeUntilAttack -= deltaTime;
            if (TimeUntilAttack <= 0)
            {
                var target = GetRandomTarget();
                if (target != null)
                {
                    AttackUnit(target);
                    ResetTimeUntilAttack();
                }
            }
        }

        /// <summary>
        /// Called in the beginning of the first tick
        /// </summary>
        private void Start()
        {
            _wasStartCalled = true;
            ResetTimeUntilAttack();
        }

        private void ResetTimeUntilAttack()
        {
            TimeUntilAttack = AttackInterval;
        }

        public void ReceiveDamage(double damage, Unit source)
        {
            _root.Log($"{Name} receives {damage} damage from {source.Name}");
            damage = ApplyArmor(damage);
            damage = Math.Max(damage, 0);
            CurrentHp -= damage;
            if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                Die(source);
            }
        }

        private void Die(Unit killer)
        {
            _root.Log($"{Name} died, killer is {killer.Name}");
            _isDead = true;
            if (Side == Side.Enemies)
            {
                killer.GiveExp(ExpReward);
            }
            Death?.Invoke(this);
        }

        private Unit GetRandomTarget()
        {
            switch (Side)
            {
                case Side.Allies:
                    return _root.GetRandomFrontEnemy();
                case Side.Enemies:
                    return _root.GetRandomFrontAlly();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AttackUnit(Unit unit)
        {
            _root.Log($"{Name} attacks {unit.Name}");
            unit.ReceiveDamage(Damage, this);
            
        }

        private double ApplyArmor(double damage)
        {
            return Math.Max(damage - Armor, 0);
        }

        /// <summary>
        /// If a unit should get Exp, override this method
        /// </summary>
        /// <param name="amount"></param>
        public virtual void GiveExp(double amount)
        {
            
        }
    }
}