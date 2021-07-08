using System;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class Unit
    {
        public event Action<Unit> Death;
        
        public virtual double CurrentHp { get; protected set; }
        public virtual double MaxHp { get; }
        public virtual double Armor { get; }
        public virtual double Damage { get; }
        public virtual double AttackSpeed { get; }
        public Side Side { get; set; }
        public UnitView View { get; set; }

        protected CombatModuleRoot _root;

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
        protected Unit()
        {
            Side = Side.Allies;
        }

        public virtual void Tick()
        {
            
        }

        public void ReceiveDamage(int damage)
        {
            damage = Mathf.Max(damage, 0);
            CurrentHp -= damage;
            if (CurrentHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GameObject.Destroy(View.gameObject);
            Death?.Invoke(this);
        }
    }
}