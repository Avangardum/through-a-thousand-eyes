using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ThroughAThousandEyes.WebModule
{
    public class Food : MonoBehaviour
    {
        public enum Rarity
        {
            None = 0,
            Common = 1,
            Rare = 2,
            Shiny = 3
        }
        
        public event Action<Food> EDeath;
        public event Action<Food> EEscape;

        [SerializeField] private GameObject hpBar;
        [SerializeField] private float hpBarMinXPos;
        [SerializeField] private float hpBarMaxXPos;
        [SerializeField] private float hpBarMinXScale;
        [SerializeField] private float hpBarMaxXScale;
        
        private double _currentHp;
        public double MaxHp { get; private set; }
        private float _currentTimeUntilEscape = 15;
        private float _maxTimeUntilEscape = 15;
        private bool _isDead;
        private bool _hasEscaped;
        public bool IsBig { get; private set; }
        private WebModuleRoot _root;
        private List<Spider> _attackingSpiders = new List<Spider>();
        private Rarity _rarity;

        private double Hp
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

        public float ExpMultiplier
        {
            get
            {
                switch (_rarity)
                {
                    case Rarity.Common:
                        return 1;
                    case Rarity.Rare:
                        return _root.Data.RareFoodExpMultiplier;
                    case Rarity.Shiny:
                        return _root.Data.ShinyFoodExpMultiplier;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public int AttackingSpidersCount => _attackingSpiders.Count;

        private void Die()
        {
            EDeath?.Invoke(this);
            Destroy(gameObject);
            _isDead = true;
            if (IsBig)
            {
                double experienceToEachSpider = (MaxHp * ExpMultiplier) / _attackingSpiders.Count;
                foreach (Spider spider in _attackingSpiders)
                {
                    spider.Experience += experienceToEachSpider;
                }
            }
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

        public void Initialize(WebModuleRoot root, bool isBig, Rarity rarity)
        {
            _root = root;
            IsBig = isBig;
            _rarity = rarity;
            
            MaxHp = (isBig ? _root.Data.BigFoodBaseHp : _root.Data.NormalFoodBaseHp) * _root.UpgradeManager.FattyInsects.HpMultiplier;
            _currentHp = MaxHp;
            SetHpBar((float)(_currentHp / MaxHp));

            _maxTimeUntilEscape =
                    IsBig
                    ? _root.Data.BigFoodBaseEscapeTime + _root.UpgradeManager.StickierWeb.ExtraTime
                    : _root.Data.NormalFoodBaseEscapeTime;
            _currentTimeUntilEscape = _maxTimeUntilEscape;

            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            switch (rarity)
            {
                case Rarity.Common:
                    renderer.color = Color.white;
                    break;
                case Rarity.Rare:
                    renderer.color = Color.green;
                    break;
                case Rarity.Shiny:
                    renderer.color = Color.yellow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
            }
        }

        public void Initialize(WebModuleRoot root, bool isBig)
        {
            bool isRare = root.UpgradeManager.RareFoodSpawnChangeUpgrade.Chance >= Random.Range(0f, 1f) && 
                          root.UpgradeManager.RareFoodSpawnChangeUpgrade.Chance > 0;
            bool isShiny = root.UpgradeManager.ShinyFoodSpawnChangeUpgrade.Chance > Random.Range(0f, 1f) &&
                           root.UpgradeManager.ShinyFoodSpawnChangeUpgrade.Chance > 0;
            Rarity rarity;
            if (isShiny)
            {
                rarity = Rarity.Shiny;
            }
            else if (isRare)
            {
                rarity = Rarity.Rare;
            }
            else
            {
                rarity = Rarity.Common;
            }
            Initialize(root, isBig, rarity);
        }

        private void FixedUpdate()
        {
            if (_currentTimeUntilEscape <= 0)
            {
                Escape();
            }
            
            _currentTimeUntilEscape -= Time.fixedDeltaTime;
        }

        public void DealDamage(double damage, Spider source, out double damageActuallyDealt, out bool isFatal)
        {
            damageActuallyDealt = Math.Min(damage, Hp);
            if (IsBig && !_attackingSpiders.Contains(source))
            {
                _attackingSpiders.Add(source);
            }
            Hp -= damage;
            isFatal = _isDead;
        }
    }
}