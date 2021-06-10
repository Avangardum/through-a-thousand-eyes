using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughAThousandEyes.MainModule
{
    public class MainModuleFacade : IModuleFacade
    {
        public Inventory Inventory { get; private set; }

        private List<IModuleFacade> _moduleFacades;
        private MainModuleUnityInterface _unityInterface;

        public void InitializeModule(MainModuleFacade mainModuleFacade, bool isLoadingSavedGame, string saveData = "")
        {
            _unityInterface = new GameObject("Main Module Unity Interface").AddComponent<MainModuleUnityInterface>();
            _unityInterface.EFixedUpdate += OnFixedUpdate;
            Inventory = new Inventory();
            if (isLoadingSavedGame)
            {
                throw new NotImplementedException();
            }
        }

        public string SaveModule()
        {
            throw new System.NotImplementedException();
        }

        public void Tick(float deltaTime)
        {
            
        }

        public void InitializeGame(bool isLoadingSavedGame, string saveData = "")
        {
            // Create modules facades
            _moduleFacades = new List<IModuleFacade>();
            _moduleFacades.Add(this);
            
            // Initialize modules using facades
            foreach (var facade in _moduleFacades)
            {
                facade.InitializeModule(this, isLoadingSavedGame, saveData);
            }
        }

        public void SaveGame()
        {
            throw new System.NotImplementedException();
        }

        private void OnFixedUpdate()
        {
            foreach (var facade in _moduleFacades)
            {
                facade.Tick(Time.fixedDeltaTime);
            }
        }
    }
}