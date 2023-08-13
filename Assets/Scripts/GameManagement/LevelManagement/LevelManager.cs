using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement.LevelManagement
{
    public interface ILevelManager
    {
        public void LoadFirstLevel();
        public void LoadUIScene();
    }
    
    public class LevelManager : MonoBehaviour, ILevelManager
    {
        public void LoadFirstLevel()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync(0);
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