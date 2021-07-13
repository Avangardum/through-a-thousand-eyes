using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ThroughAThousandEyes.AdventureMapModule
{
    public class AdventureMapModuleRoot : MonoBehaviour, IFocusable
    {
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private AdventureMapModuleUI ui;

        public AdventureMapModuleFacade Facade;
        
        public void OnGetFocus()
        {
            ui.gameObject.SetActive(true);
        }

        public void OnLoseFocus()
        {
            ui.gameObject.SetActive(false);
        }

        public Vector3 GetCameraPosition()
        {
            return cameraPosition.position;
        }

        public void Initialize(AdventureMapModuleFacade facade, JObject saveData)
        {
            Facade = facade;
            ui.Initialize(this);
        }
    }
}