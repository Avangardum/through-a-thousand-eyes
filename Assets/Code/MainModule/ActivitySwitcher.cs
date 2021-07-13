namespace ThroughAThousandEyes.MainModule
{
    public class ActivitySwitcher
    {
        private MainModuleRoot _root;

        public ActivitySwitcher(MainModuleRoot root)
        {
            _root = root;
        }

        public void SwitchToWeb()
        {
            _root.focusManager.FocusOnWeb();
        }

        public void StartEndlessFight()
        {
            _root.focusManager.FocusOnCombat();
            _root.Facade._combatModuleFacade.StartEndlessFight();
        }
    }
}