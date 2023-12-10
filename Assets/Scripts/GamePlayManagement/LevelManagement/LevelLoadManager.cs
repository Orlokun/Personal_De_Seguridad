using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlayManagement.LevelManagement
{
    public interface ILevelManager
    {
        public void LoadAdditiveLevel(LevelIndexId lvl);
        public void UnloadScene(LevelIndexId lvl);
        public void ClearNotOfficeScenes();
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
        public void LoadAdditiveLevel(LevelIndexId lvl)
        {
            SceneManager.LoadScene((int)lvl, LoadSceneMode.Additive);
        }

        public void UnloadScene(LevelIndexId lvl)
        {
            if (!SceneManager.GetSceneByBuildIndex((int) lvl).isLoaded)
            {
                return;
            }
            SceneManager.UnloadSceneAsync((int)lvl);
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