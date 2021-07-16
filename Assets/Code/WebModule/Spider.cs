using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughAThousandEyes.WebModule
{
    public class Spider : MonoBehaviour
    {
        [SerializeField] private TextMeshPro levelText;
        
        private WebModuleRoot _root;
        private Food _target;
        private bool _hasTarget;
        private float _speed = 1;
        private long _level = 1; // Only for small spiders
        private float _currentAttackCooldown;
        private double _experience;
        private double _silk = 0;
        public bool IsMainSpider { get; private set; }

        private double ExperienceNeededForNextLevel =>
            _root.Data.ExperienceToLevelUpBase + _root.Data.ExperienceToLevelUpAddition * (Level - 1);

        public double Experience
        {
            get => _experience;
            set
            {
                _experience = value;
                while (_experience >= ExperienceNeededForNextLevel)
                {
                    _experience -= ExperienceNeededForNextLevel;
                    Level++;
                }
            }
        }

        public long Level
        {
            get => IsMainSpider ? _root.Facade.MainSpiderStats.Level : _level;
            set
            {
                if (IsMainSpider)
                {
                    _root.Facade.MainSpiderStats.Level = value;
                }
                else
                {
                    _level = value;
                }
            }
        }

        public void Initialize(WebModuleRoot root, bool isMainSpider)
        {
            _root = root;
            _currentAttackCooldown = _root.Data.AttackInterval;
            IsMainSpider = isMainSpider;
        }

        private void FixedUpdate()
        {
            if (!_hasTarget)
            {
                GetTarget();
            }

            if (_hasTarget)
            {
                var vectorToTarget = _target.transform.position - this.transform.position;
                var distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);
                var extraDistance = Mathf.Max(distanceToTarget - _root.Data.AttackRange, 0);
                if (extraDistance > 0)
                {
                    var movement = Mathf.Min(_speed * Time.fixedDeltaTime, extraDistance);
                    var movementVector = vectorToTarget.normalized * movement;
                    transform.Translate(movementVector, Space.World);
                }
                else
                {
                    if (_currentAttackCooldown <= 0)
                    {
                        double damage = Level * _root.UpgradeManager.FeedingGrounds.DamageMultiplier;
                        if (_target.IsBig)
                        {
                            damage *= 1 + 
                                (_root.UpgradeManager.CollectiveFeeding.DamageIncreasePerSpider * (_target.AttackingSpidersCount - 1));
                        }
                        Attack(_target, damage);
                    }
                }
            }

            _silk += (double)(Level * Time.deltaTime);
            if (_silk >= 1)
            {
                long silkToTransfer = (long)Math.Floor(_silk);
                _silk -= silkToTransfer;
                _root.Facade.Inventory.Silk += silkToTransfer;
            }
            
            
            _currentAttackCooldown -= Time.fixedDeltaTime;
            levelText.text = Level.ToString();

            AcidicWebFixedUpdate();
        }

        private void AcidicWebFixedUpdate()
        {
            foreach (var food in _root._foods)
            {
                Attack(food, _root.UpgradeManager.AcidicWeb.DamagePerSecond * Time.fixedDeltaTime, true);
            }
        }

        private void GetTarget()
        {
            var availableTargets = // Target is available if it is not targeted by other spiders or if is is big
                _root._foods.Where(x => _root._spiders.All(y => y._target != x) || x.IsBig);
            if (availableTargets.Any())
            {
                Food closestTarget = null;
                float closestTargetSqrDistance = Mathf.Infinity;
                foreach (var availableTarget in availableTargets)
                {
                    var sqrDistance = (availableTarget.transform.position - this.transform.position).sqrMagnitude;
                    if (sqrDistance < closestTargetSqrDistance)
                    {
                        closestTarget = availableTarget;
                        closestTargetSqrDistance = sqrDistance;
                    }
                }

                _target = closestTarget;
                _hasTarget = true;
                _target.EDeath += OnTargetDeletion;
                _target.EEscape += OnTargetDeletion;
            }
        }

        private void OnTargetDeletion(Food target)
        {
            _target.EDeath -= OnTargetDeletion;
            _target.EEscape -= OnTargetDeletion;

            _target = null;
            _hasTarget = false;
        }

        private void Attack(Food target, double damage, bool isAcidicWeb = false)
        {
            double damageActuallyDealt;
            bool isFatal;
            target.DealDamage(damage, this, out damageActuallyDealt, out isFatal);
            Experience += damageActuallyDealt * target.ExpMultiplier;
            if (isFatal && !target.IsBig)
            {
                Experience += target.MaxHp * target.ExpMultiplier;
            }

            if (!isAcidicWeb)
            {
                _currentAttackCooldown = _root.Data.AttackInterval;
            }
        }
    }
}