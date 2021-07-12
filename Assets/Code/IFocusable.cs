using UnityEngine;

namespace ThroughAThousandEyes
{
    public interface IFocusable
    {
        void OnGetFocus();
        void OnLoseFocus();

        Vector3 GetCameraPosition();
    }
}