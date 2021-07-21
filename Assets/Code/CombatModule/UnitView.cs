using System;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class UnitView : MonoBehaviour
    {
        [HideInInspector] public int PositionIndex;
        private Unit _unit;

        [SerializeField] private GameObject hpBar;
        [SerializeField] private float attackImpactPoint;
        [SerializeField] private float hpBarMinXPos;
        [SerializeField] private float hpBarMaxXPos;
        [SerializeField] private float hpBarMinXScale;
        [SerializeField] private float hpBarMaxXScale;
        
        private Animator _animator;
        private bool _isAttackAnimationPlaying;
        private readonly int _attackHash = Animator.StringToHash("Attack"); 

        private void Update()
        {
            SetHpBar((float)_unit.CurrentHp / (float)_unit.MaxHp);
            if (_isAttackAnimationPlaying)
            {
                if (_unit.TimeUntilAttack > attackImpactPoint)
                {
                    _isAttackAnimationPlaying = false;
                }
            }
            else
            {
                if (_unit.TimeUntilAttack <= attackImpactPoint)
                {
                    _animator.Play(_attackHash);
                    _isAttackAnimationPlaying = true;
                }
            }
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

        public void Initialize(Unit unit)
        {
            _unit = unit;
            _unit.Death += OnDeath;
            _animator = GetComponent<Animator>();
        }

        private void OnDeath(Unit unit)
        {
            Destroy(gameObject);
        }
    }
}