using Newtonsoft.Json.Linq;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    public class CombatModuleFacade : IModuleFacade, IFocusable, ITickable, ISavable
    {
        public MainModuleFacade MainModuleFacade;
        public bool IsCombatActive => _root.IsCombatActive;
        public int KingdomDefenceStagePassed => _root.KingdomDefenceStagesPassed;
        
        private CombatModuleRoot _root;
        
        public void InitializeModule(MainModuleFacade mainModuleFacade, JObject saveData = null)
        {
            MainModuleFacade = mainModuleFacade;
            _root = Object.FindObjectOfType<CombatModuleRoot>();
            _root.Initialize(this, saveData);
        }

        public JObject SaveModuleToJson() => _root.SaveModuleToJson();

        public string GetJsonPropertyName()
        {
            return "combatModule";
        }

        public void Tick(float deltaTime)
        {
            _root.Tick(deltaTime);
        }
        
        public void OnGetFocus()
        {
            _root.OnGetFocus();
        }

        public void OnLoseFocus()
        {
            _root.OnLoseFocus();
        }

        public Vector3 GetCameraPosition()
        {
            return _root.GetCameraPosition();
        }

        public void StartEndlessFight() => _root.StartEndlessFight();

        public void StartStressTest() => _root.StartStressTest();

        public void StartKingdomDefence() => _root.StartKingdomDefence();

        public void StartKingdomAttack() => _root.StartKingdomAttack();
    }
}