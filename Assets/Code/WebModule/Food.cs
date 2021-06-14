using System;
using UnityEngine;

namespace ThroughAThousandEyes.WebModule
{
    public class Food : MonoBehaviour
    {
        public event Action<Food> Death;
        public event Action<Food> Escape;

        [SerializeField] private GameObject hpBar;
        [SerializeField] private float hpBarMinXPos;
        [SerializeField] private float hpBarMaxXPos;
        [SerializeField] private float hpBarMinXScale;
        [SerializeField] private float hpBarMaxXScale;
        
        private float _currentHp = 10;
        private float _maxHp = 10;

        public float Hp
        {
            get => _currentHp;
            set
            {
                _currentHp = value;
                SetHpBar(_currentHp / _maxHp);
                if (_currentHp <= 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            Death?.Invoke(this);
            Destroy(gameObject);
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
            SetHpBar(Hp / _maxHp);
        }
    }
}