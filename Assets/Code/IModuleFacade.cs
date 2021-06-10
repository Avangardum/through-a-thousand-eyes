using ThroughAThousandEyes.MainModule;

namespace ThroughAThousandEyes
{
    public interface IModuleFacade
    {
        void InitializeModule(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "");
        string SaveModule();
        void Tick(float deltaTime);
    }
}
