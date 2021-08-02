using UnityEngine;

namespace ThroughAThousandEyes.MainSpiderUpgradingModule
{
    public class MainSpiderUpgradingModuleRoot : MonoBehaviour, IFocusable
    {
        [SerializeField] private Transform cameraPosition;
        
        public void OnGetFocus()
        {
            throw new System.NotImplementedException();
        }

        public void OnLoseFocus()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetCameraPosition()
        {
            return cameraPosition.position;
        }
    }
}