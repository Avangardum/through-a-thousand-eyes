using System;
using TMPro;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleUI : MonoBehaviour
    {
        private CombatModuleRoot _root;
        
        [SerializeField] private TextMeshProUGUI _idleAllyCounterText;
        [SerializeField] private TextMeshProUGUI _idleEnemyCounterText;

        public void Initialize(CombatModuleRoot root)
        {
            _root = root;
        }

        private void Update()
        {
            _idleAllyCounterText.text = _root.IdleAlliesCount.ToString();
            _idleEnemyCounterText.text = _root.IdleEnemiesCount.ToString();
        }
    }
}