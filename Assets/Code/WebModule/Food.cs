using System;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class Food : MonoBehaviour
    {
        public event Action<Food> EDeath;
        public event Action<Food> EEscape;

        [SerializeField] private GameObject hpBar;
        [SerializeField] private float hpBarMinXPos;
        [SerializeField] private float hpBarMaxXPos;
        [SerializeField] private float hpBarMinXScale;
        [SerializeField] private float hpBarMaxXScale;
        
        private decimal _currentHp = 10;
        public decimal MaxHp { get; private set; } = 10;
        private float _currentTimeUntilEscape = 15;
        private float _maxTimeUntilEscape = 15;
        private bool _isDead;
        private bool _hasEscaped;

        public decimal Hp
        {
            get => _currentHp;
            set
            {
                _currentHp = value;
                SetHpBar((float)(_currentHp / MaxHp));
                if (_currentHp <= 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            EDeath?.Invoke(this);
            Destroy(gameObject);
            _isDead = true;
        }

        private void Escape()
        {
            EDeath?.Invoke(this);
            Destroy(gameObject);
            _hasEscaped = true;
        }

        private void SetHpBar(float amount)
        {
            var pos = hpBar.transform.localPosition;
            pos.x = Mathf.Lerp(hpBarMinXPos, hpBarMaxXPos, amount);
            hpBar.transform.localPosition = pos;

            var scale = hpBar.transform.localScale;
            scale.x = Mathf.Lerp(hpBarMinXScale, hpBarMaxXScale, amount);
            hpBar.transform.localScale = scale;
        }

        public void Initialize()
        {
            SetHpBar((float)(_currentHp / MaxHp));
        }

        private void FixedUpdate()
        {
            if (_currentTimeUntilEscape <= 0)
            {
                Escape();
            }
            
            _currentTimeUntilEscape -= Time.fixedDeltaTime;
        }

        public void DealDamage(decimal damage, out decimal damageActuallyDealt, out bool isFatal)
        {
            damageActuallyDealt = Math.Min(damage, Hp);
            Hp -= damage;
            isFatal = _isDead;
        }
    }
}