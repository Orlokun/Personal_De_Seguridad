using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
namespace Players_NPC.NPC_Management.Customer_Management
{
    public class CustomersInSceneManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] mClientPrefabs;
        [SerializeField] private Transform mStartPosition;
        private int _mInstantiationFrequency = 2;
        
        
        private void Start()
        {
            StartCoroutine(StartInstantiatingClients());
        } 

        private IEnumerator StartInstantiatingClients()
        {
            var infiniteTrue = true;
            while (infiniteTrue)
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
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(0, mClientPrefabs.Length - 1);
            return Instantiate(mClientPrefabs[randomIndex], mStartPosition.position, new Quaternion(0,80,0,0));
        }
    }
}