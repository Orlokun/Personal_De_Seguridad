using System;
using System.Collections;
using UnityEngine;

namespace GameDirection.DayLevelSceneManagers
{
    public class DayZeroIntroScene : IIntroSceneOperator
    {
        public bool IsInitialized => mInitialized;
        private bool mInitialized;
        private IGameDirector _mGameDirector;

        
        public void Initialize(IGameDirector injectionClass)
        {
            if (IsInitialized)
            {
                return;
            }
            if (injectionClass == null)
            {
                Debug.Log("[IntroSceneManager.InitializeWithArg] Injection must not be null");
                return;
            }
            _mGameDirector = injectionClass;
            mInitialized = true;
        }

        public IEnumerator StartIntroScene()
        {
            throw new NotImplementedException();
        }
    }
}