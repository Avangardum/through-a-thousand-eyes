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
        private float _level = 1;
        private float _currentAttackCooldown;
        
        
        public void Initialize(WebModuleRoot root)
        {
            _root = root;
            _currentAttackCooldown = _root.AttackInterval;
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
                var extraDistance = Mathf.Max(distanceToTarget - _root.AttackRange, 0);
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
                        // Attack
                        _target.Hp -= _level;
                        _currentAttackCooldown = _root.AttackInterval;
                    }
                }
            }

            _currentAttackCooldown -= Time.fixedDeltaTime;
            levelText.text = _level.ToString();
        }

        private void GetTarget()
        {
            var availableTargets = _root._foods.Where(x => !_root._spiders.Any(y => y._target == x));
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
    }
}