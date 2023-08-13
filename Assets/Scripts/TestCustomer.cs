using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TestCustomer : MonoBehaviour
{
    [SerializeField] private int mNumberOfDestinations;
    [SerializeField] private Transform mPayingPosition;
    
    
    private int _currentDestination = 0;
    private NavMeshAgent _navMeshAgent;
    
    private ShopPositionsManager _positionsManager;
    
    private Vector3[] _mTargetPositions;
    
    private Vector3 _mInitialPosition;
    private Vector3 _mPayingPosition;
    
    private bool _mFinishedPath = false;
    private BaseCustomerStatus _mCustomerStatus;
    private void Awake()
    {
        _mInitialPosition = transform.position;
        _mPayingPosition = mPayingPosition.position;
        _mCustomerStatus = BaseCustomerStatus.Shopping;
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _positionsManager = FindObjectOfType<ShopPositionsManager>();
    }
    private void Start()
    {
        _mTargetPositions = new Vector3[mNumberOfDestinations];
        _mTargetPositions = _positionsManager.GetRandomPositions(mNumberOfDestinations);

        _navMeshAgent.destination = _mTargetPositions[_currentDestination];
        Debug.Log($"[Awake] Initial Position: {_mInitialPosition}. ");
    }

    private void Update()
    {
        switch (_mCustomerStatus)
        {
            case BaseCustomerStatus.Shopping:
                Shop();
                break;
            case BaseCustomerStatus.Paying:
                GoToPay();
                break;
            case BaseCustomerStatus.Leaving:
                Leave();
                break;
            case BaseCustomerStatus.Stealing:
                break;
            case BaseCustomerStatus.Running:
                break;
            case BaseCustomerStatus.Detained:
                break;
        }
    }

    #region Shopping
    private void Shop()
    {
        if (_navMeshAgent.remainingDistance < .5f && !_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = true;
            StartCoroutine(GoToNextPoint(Random.Range(2,10)));
        }
    }
    private IEnumerator GoToNextPoint(float timeLength)
    {
        yield return new WaitForSeconds(timeLength);
        _currentDestination++;
        if(_currentDestination >= mNumberOfDestinations)
        {
            Debug.Log("[GoToNextPoint] Going to Pay");
            _navMeshAgent.SetDestination(mPayingPosition.position);
            _mCustomerStatus = BaseCustomerStatus.Paying;
            _navMeshAgent.isStopped = false;
            yield break;
        }
        Debug.Log("[GoToNextPoint] Going to Next Point as usual");
        var destinationCorrectlySet = _navMeshAgent.SetDestination(_mTargetPositions[_currentDestination]);
        _navMeshAgent.isStopped = !destinationCorrectlySet;
    }

    #endregion

    #region Paying
    private void GoToPay()
    {
        if (_navMeshAgent.remainingDistance < .5f && !_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = true;
            StartCoroutine(PayAndLeave(Random.Range(2,10)));
        }
    }
    private IEnumerator PayAndLeave(float timePaying)
    {
        yield return new WaitForSeconds(timePaying);
        _mCustomerStatus = BaseCustomerStatus.Leaving;
        _navMeshAgent.SetDestination(_mInitialPosition);
        _navMeshAgent.isStopped = false;
    }
    #endregion

    #region Leaving

    private void Leave()
    {
        if (_navMeshAgent.remainingDistance < .5f && !_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = true;
            gameObject.SetActive(false);
        }
    }
    

    #endregion
}

public enum BaseCustomerStatus
{
    Shopping,
    Paying,
    Leaving,
    Stealing,
    Running,
    Detained,
}
