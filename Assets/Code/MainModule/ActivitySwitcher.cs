namespace ThroughAThousandEyes.MainModule
{
    public class ActivitySwitcher
    {
        private readonly MainModuleRoot _root;

        public ActivitySwitcher(MainModuleRoot root)
        {
            _root = root;
        }

        public void SwitchToWeb()
        {
            _root.FocusManager.FocusOnWeb();
        }

        public void StartEndlessFight()
        {
            _root.FocusManager.FocusOnCombat();
            _root.CombatModuleFacade.StartEndlessFight();
        }

        public void StartStressTestFight()
        {
            _root.FocusManager.FocusOnCombat();
            _root.CombatModuleFacade.StartStressTest();
        }

        public void SwitchToAdventureMap()
        {
            _root.FocusManager.FocusOnAdventureMap();
        }

        public void SwitchToMainSpiderUpgrading()
        {
            _root.FocusManager.FocusOnMainSpiderUpgrading();
        }

        public void StartKingdomDefence()
        {
            _root.FocusManager.FocusOnCombat();
            _root.CombatModuleFacade.StartKingdomDefence();
        }
    }
}