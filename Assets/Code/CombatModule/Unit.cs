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

        protected readonly CombatModuleRoot _root;
        private double _timeUntilAttack;
        private bool _wasStartCalled;
        private bool _isDead;

        private double AttackInterval => 1 / AttackSpeed;
        public virtual string Name => GetType().ToString().Split('.').Last();

        public Unit(CombatModuleRoot root, double maxHp, double armor, double damage, double attackSpeed, Side side)
        {
            _root = root;
            MaxHp = maxHp;
            CurrentHp = MaxHp;
            Armor = armor;
            Damage = damage;
            AttackSpeed = attackSpeed;
            Side = side;
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
            
            if (_wasStartCalled)
            {
                Start();
            }

            _timeUntilAttack -= deltaTime;
            if (_timeUntilAttack <= 0)
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
            _timeUntilAttack = AttackInterval;
        }

        public void ReceiveDamage(double damage)
        {
            _root.Log($"{Name} recieves {damage} damage");
            damage = ApplyArmor(damage);
            damage = Math.Max(damage, 0);
            CurrentHp -= damage;
            if (CurrentHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            _root.Log($"{Name} died");
            _isDead = true;
            Object.Destroy(View.gameObject);
            Death?.Invoke(this);
        }

        private Unit GetRandomTarget()
        {
            switch (Side)
            {
                case Side.Allies:
                    return _root.GetRandomEnemy();
                case Side.Enemies:
                    return _root.GetRandomAlly();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AttackUnit(Unit unit)
        {
            _root.Log($"{Name} attacks {unit.Name}");
            unit.ReceiveDamage(Damage);
            
        }

        private double ApplyArmor(double damage)
        {
            return Math.Max(damage - Armor, 0);
        }
    }
}