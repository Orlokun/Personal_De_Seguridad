using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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
        if (!FindObjectOfType<TestCustomer>())
        {
            yield break;
        }
        mTarget = FindObjectOfType<TestCustomer>().transform;
        m_agentController.destination = mTarget.position;
    }
}
