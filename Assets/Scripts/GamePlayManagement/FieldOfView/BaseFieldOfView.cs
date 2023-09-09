using System.Collections;
using UnityEngine;

namespace GamePlayManagement.FieldOfView
{
    public class BaseFieldOfView : MonoBehaviour
    {
        public float radius;
        [Range(0,360)]
        public float angle;
        private float delay = 0.2f;

        public GameObject foundObject;

        public LayerMask customerLayerMask;
        public LayerMask obstructionMask;

        public bool isCustomerFound;

        private void Start()
        {
            StartCoroutine(FoVRoutine());
        }

        private IEnumerator FoVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(delay);
            while (true)
            {
                yield return wait;
                CheckFieldOfView();
            }
        }

        private void CheckFieldOfView()
        {
            Collider[] foundCustomers = Physics.OverlapSphere(transform.position, radius, customerLayerMask);
            
            if (foundCustomers.Length != 0)
            {
                foreach (var customerCollider in foundCustomers)
                {
                    var customerTransform = customerCollider.transform;
                    var directionToTarget = (customerTransform.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        var distanceToTarget = Vector3.Distance(transform.position, customerTransform.position);
                        if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                        {
                            isCustomerFound = true;
                        }
                        else
                        {
                            isCustomerFound = false;
                        }
                    }
                    else
                    {
                        isCustomerFound = false;
                    }
                }
            }
            else if (isCustomerFound)
            {
                isCustomerFound = false;
            }
        }
    }
}