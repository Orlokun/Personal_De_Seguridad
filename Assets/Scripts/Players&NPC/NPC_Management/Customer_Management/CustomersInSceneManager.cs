using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
        public List<NavMeshAgent> NavAgents = new List<NavMeshAgent>();

        [SerializeField] private ObstacleAvoidanceType AvoidanceType;
        [SerializeField] private float AgentSpeed;
        [SerializeField] private float AgentRadius;
        
        [Header("NavMesh Configurations")]
        public float AvoidancePredictionTime;
        public int PathfindingIterationsPerFrame = 100;

        
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
                _mInstantiationFrequency = Random.Range(8, 15);
                var randomPrefabInstantiated = GetRandomClientPrefab();
                var navMesh = randomPrefabInstantiated.GetComponent<NavMeshAgent>();
                navMesh.speed = 3.5f;
                navMesh.avoidancePriority = 50;
                SceneManager.MoveGameObjectToScene(randomPrefabInstantiated, SceneManager.GetSceneByName("Level_One"));
                Debug.Log($"Instantiated Object: {randomPrefabInstantiated.name}. Waiting {_mInstantiationFrequency}");
                yield return new WaitForSeconds(_mInstantiationFrequency);
            }
        }

        private void Update()
        {
            NavMesh.avoidancePredictionTime = AvoidancePredictionTime;
            NavMesh.pathfindingIterationsPerFrame = PathfindingIterationsPerFrame;
        }

        private GameObject GetRandomClientPrefab()
        {
            /*
            Random.InitState(DateTime.Now.Millisecond);
            var randomIndex = Random.Range(0, mClientPrefabs.Length - 1);
            */
            return Instantiate(mClientPrefabs[0], mStartPosition.position, new Quaternion(0,80,0,0));
        }
        
        private void SetupAgent(NavMeshAgent Agent)
        {
            Agent.obstacleAvoidanceType = AvoidanceType;
            Agent.radius = AgentRadius;
            Agent.speed = AgentSpeed;
            Agent.avoidancePriority = 50;
            NavAgents.Add(Agent);
        }
    }
}