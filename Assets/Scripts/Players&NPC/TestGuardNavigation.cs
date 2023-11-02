using System.Collections;
using Players_NPC.NPC_Management.Customer_Management;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Players_NPC
{
    public class TestGuardNavigation : MonoBehaviour
    {
        private NavMeshAgent m_agentController;

        [SerializeField] private Transform mTarget;

        // Start is called before the first frame update
        void Awake()
        {
            m_agentController = GetComponent<NavMeshAgent>();
            if (!m_agentController)
            {
                Debug.LogError("No Nav Mesh Agent Component Attached. Return Void");
                return;
            }

            StartCoroutine(WaitSecondsAndAssignTarget(Random.Range(6, 20)));
        }

        private IEnumerator WaitSecondsAndAssignTarget(int seconds)
        {
            yield return new WaitForSeconds(seconds);
            Debug.Log($"Waited {seconds} seconds");
            if (!FindObjectOfType<BaseCustomer>())
            {
                yield break;
            }
            mTarget = FindObjectOfType<BaseCustomer>().transform;
            m_agentController.destination = mTarget.position;
        }
    }
}