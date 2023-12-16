using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
namespace Players_NPC.NPC_Management.Customer_Management
{
    public interface ICustomersInSceneManager
    {
        void ToggleSpawning(bool isSpawning);
    }

    public class CustomersInSceneManager : MonoBehaviour, ICustomersInSceneManager
    {
        [SerializeField] private GameObject[] mClientPrefabs;
        [SerializeField] private Transform mStartPosition;
        private int _mInstantiationFrequency = 2;
        private bool _mIsSpawning = false;
        private Coroutine customersCoroutine;
        
        public void ToggleSpawning(bool isSpawning)
        {
            if (isSpawning && _mIsSpawning)
            {
                return;
            }
            _mIsSpawning = isSpawning;
            switch (_mIsSpawning)
            {
                case false:
                    StopCoroutine(customersCoroutine);
                    break;
                case true:
                    customersCoroutine = StartCoroutine(StartInstantiatingClients());
                    break;
            }
        }
        
        private IEnumerator StartInstantiatingClients()
        {
            while (_mIsSpawning)
            {
                Random.InitState(DateTime.Now.Millisecond);
                _mInstantiationFrequency = Random.Range(10, 20);
                var randomPrefabInstantiated = GetRandomClientPrefab();
                SceneManager.MoveGameObjectToScene(randomPrefabInstantiated, SceneManager.GetSceneByName("Level_One"));
                Debug.Log($"Instantiated Object: {randomPrefabInstantiated.name}. Waiting {_mInstantiationFrequency}");
                yield return new WaitForSeconds(_mInstantiationFrequency);
            }
        }

        private GameObject GetRandomClientPrefab()
        {
            /*
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(0, mClientPrefabs.Length - 1);
            */
            return Instantiate(mClientPrefabs[0], mStartPosition.position, new Quaternion(0,80,0,0));
        }
    }
}