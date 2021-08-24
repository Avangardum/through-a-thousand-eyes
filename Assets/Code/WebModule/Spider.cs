using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughAThousandEyes.WebModule
{
    public class Spider : MonoBehaviour
    {
        private const float AttackRangeAcceptableInaccuracy = 0.001f;
        
        public enum StateEnum
        {
            None = 0,
            Weaving = 1,
            Feeding = 2
        }
        
        [SerializeField] private TextMeshPro levelText;
        
        private WebModuleRoot _root;
        private Food _target;
        private bool _hasTarget;
        private float _speed = 1;
        private long _level = 1; // Only for small spiders
        private float _currentAttackCooldown;
        private double _experience; // Only for small spiders
        private double _silk = 0;
        private StateEnum _state = StateEnum.Feeding;
        public bool IsMainSpider { get; private set; }

        public StateEnum State
        {
            get => _state;
            set
            {
                _state = value;
                switch (State)
                {
                    case StateEnum.Weaving:
                        RemoveCurrentTarget();
                        break;
                    case StateEnum.Feeding:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private double ExperienceNeededForNextLevel =>
            _root.Facade.MainModuleFacade.Data.ExperienceToGetLevelN.GetElement(Level + 1);

        public double Experience
        {
            get => IsMainSpider ? _root.Facade.MainSpiderStats.Experience : _experience;
            set
            {
                if (IsMainSpider)
                {
                    _root.Facade.MainSpiderStats.Experience = value;
                }
                else
                {
                    _experience = value;
                    while (_experience >= ExperienceNeededForNextLevel)
                    {
                        _experience -= ExperienceNeededForNextLevel;
                        Level++;
                    }
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
            switch (State)
            {
                case StateEnum.Weaving:
                    WeavingFixedUpdate();
                    break;
                case StateEnum.Feeding:
                    FeedingFixedUpdate();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _currentAttackCooldown -= Time.fixedDeltaTime;
            levelText.text = Level.ToString();

            if (IsMainSpider)
            {
                AcidicWebFixedUpdate();
            }
        }

        private void FeedingFixedUpdate()
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
                if (extraDistance > AttackRangeAcceptableInaccuracy)
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
                            damage *= 1 + (_root.UpgradeManager.CollectiveFeeding.DamageIncreasePerSpider * (_target.AttackingSpidersCount - 1));
                        }
                        Attack(_target, damage);
                    }
                }
            }
        }

        private void WeavingFixedUpdate()
        {
            _silk += (double)(Level * Time.deltaTime);
            if (_silk >= 1)
            {
                long silkToTransfer = (long)Math.Floor(_silk);
                _silk -= silkToTransfer;
                _root.Facade.Inventory.Silk += silkToTransfer;
            }
        }

        private void AcidicWebFixedUpdate()
        {
            foreach (var food in _root._foods.ToArray())
            {
                Attack(food, _root.UpgradeManager.AcidicWeb.DamagePerSecond * Time.fixedDeltaTime, true);
            }
        }

        private void GetTarget()
        {
            var availableTargets = // Target is available if it is not targeted by other spiders or if it is big
                _root._foods.Where(x => _root.Spiders.All(y => y._target != x) || x.IsBig);
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
                _target.EDeath += RemoveTarget;
                _target.EEscape += RemoveTarget;
            }
        }

        private void RemoveTarget(Food target)
        {
            _target.EDeath -= RemoveTarget;
            _target.EEscape -= RemoveTarget;

            _target = null;
            _hasTarget = false;
        }

        private void RemoveCurrentTarget()
        {
            if (!_hasTarget)
                return;

            RemoveTarget(_target);
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