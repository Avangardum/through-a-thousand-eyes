using System;
using System.Collections;
using System.Collections.Generic;
using ThroughAThousandEyes.MainModule;
using UnityEngine;

namespace ThroughAThousandEyes
{
    public class GameStarter : MonoBehaviour
    {
        private MainModuleFacade _mainModuleFacade;

        private void Awake()
        {
            StartGame();
        }

        private void StartGame()
        {
            _mainModuleFacade = new MainModuleFacade();
            _mainModuleFacade.InitializeGame(false);
        }
    }
}
