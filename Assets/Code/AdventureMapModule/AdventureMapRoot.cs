using UnityEngine;

namespace ThroughAThousandEyes.AdventureMapModule
{
    public class AdventureMapRoot : MonoBehaviour, IFocusable
    {
        [SerializeField] private Transform cameraPosition;
        
        public void OnGetFocus()
        {
            
        }

        public void OnLoseFocus()
        {
            
        }

        public Vector3 GetCameraPosition()
        {
            return cameraPosition.position;
        }
    }
}