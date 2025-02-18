using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlayManagement.LevelManagement
{
    public interface ILevelManager
    {
        public void ActivateScene(LevelIndexId lvl);
        public void DeactivateScene(LevelIndexId lvl);
        public void ClearNotOfficeScenes();
        public void ReturnToMainScreen();
    }

    public enum LevelIndexId
    {
        InitScene = 0,
        UILvl = 1,
        OfficeLvl = 2,
        
        //Gameplay levels threshold
        EdenLvl = 3,
    }
    
    public class LevelLoadManager : MonoBehaviour, ILevelManager
    {
        private LevelIndexId _currentGameLevel;
        public LevelIndexId CurrentGameLevel => _currentGameLevel;
        
        private Dictionary<LevelIndexId, bool> _mSceneStatus = new Dictionary<LevelIndexId, bool>();
        private void Start()
        {
            SetBaseSceneStatus();
        }

        private void SetBaseSceneStatus()
        {
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                if(_mSceneStatus.ContainsKey((LevelIndexId)i))
                {
                    continue;
                }
                var scene = SceneManager.GetSceneByBuildIndex(i);
                _mSceneStatus.Add((LevelIndexId)i, scene.isLoaded);
            }
        }

        private void LoadAdditiveLevel(LevelIndexId lvl)
        {
            if(SceneManager.GetSceneByBuildIndex((int)lvl).isLoaded)
            {
                return;
            }
            SceneManager.LoadScene((int)lvl, LoadSceneMode.Additive);
            _mSceneStatus[lvl] = true;
        }

        public void ActivateScene(LevelIndexId lvl)
        {
            if (!SceneManager.GetSceneByBuildIndex((int) lvl).isLoaded)
            {
                LoadAdditiveLevel(lvl);
                return;
            }
            if(_mSceneStatus[lvl])
            {
                return;
            }
            
            var scene = SceneManager.GetSceneByBuildIndex((int)lvl);
            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                rootObject.SetActive(true);
            }
            _mSceneStatus[lvl] = true;
        }
        
        public void DeactivateScene(LevelIndexId lvl)
        {
            if (!SceneManager.GetSceneByBuildIndex((int) lvl).isLoaded)
            {
                return;
            }
            var scene = SceneManager.GetSceneByBuildIndex((int)lvl);
            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                rootObject.SetActive(false);
            }
            _mSceneStatus[lvl] = false;
        }

        public void ReLoadScene(LevelIndexId lvl)
        {
            if (!SceneManager.GetSceneByBuildIndex((int) lvl).isLoaded)
            {
                return;
            }
            
            var scene = SceneManager.GetSceneByBuildIndex((int)lvl);
            var rootObjects = scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                rootObject.SetActive(true);
            }
        }

        public void ClearNotOfficeScenes()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneByBuildIndex(i);
                if (scene.isLoaded && scene.name != "OfficeScene" && scene.name != "UI_Scene")
                {
                    SceneManager.UnloadSceneAsync(i);
                }
            }
        }
        public void ReturnToMainScreen()
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var levelIndexId = (LevelIndexId)i;
                var scene = SceneManager.GetSceneByBuildIndex(i);
                if (_mSceneStatus[levelIndexId] && scene.name != "InitScene" && scene.name != "UI_Scene")
                { 
                    DeactivateScene((LevelIndexId)i);
                    continue;
                }
                if(scene.name == "InitScene" || scene.name == "UI_Scene")
                {
                    ActivateScene((LevelIndexId)i);
                }
            }
        }
        
        public void LoadUIScene()
        {
/*            var currentScene = SceneManager.GetActiveScene();
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var firstLevelPath = SceneUtility.GetScenePathByBuildIndex(1);
            var sceneName = System.IO.Path.GetFileNameWithoutExtension(firstLevelPath);*/
            SceneManager.LoadScene(1, LoadSceneMode.Additive);
        }
    }
}