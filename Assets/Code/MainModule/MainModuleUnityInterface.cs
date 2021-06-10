using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class MainModuleUnityInterface : MonoBehaviour
    {
        public event Action EFixedUpdate;

        private void FixedUpdate()
        {
            EFixedUpdate?.Invoke();
        }
    }
}
