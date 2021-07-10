using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class UnitView : MonoBehaviour
    {
        public int PositionIndex;
        public Unit Unit;
        
        [SerializeField] private GameObject hpBar;
        [SerializeField] private float hpBarMinXPos;
        [SerializeField] private float hpBarMaxXPos;
        [SerializeField] private float hpBarMinXScale;
        [SerializeField] private float hpBarMaxXScale;

        private void Update()
        {
            SetHpBar((float)Unit.CurrentHp / (float)Unit.MaxHp);
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
    }
}