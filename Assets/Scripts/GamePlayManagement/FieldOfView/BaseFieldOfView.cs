using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace GamePlayManagement.FieldOfView
{
    public class InGameFieldOfView : BaseFieldOfView
    {
        private Transform _objectTransform; 
        private Vector3 _objectPosition;
        private Vector3 _objectAngles;
        
        protected override void Start()
        {
            base.Start();
            _objectTransform = this.transform;
        }
        private void Update()
        {
            DrawFieldOfview();
        }

        private void DrawFieldOfview()
        {
            _objectPosition = _objectTransform.position;
            _objectAngles = _objectTransform.eulerAngles;
            
            Handles.color = Color.white;
            Handles.DrawWireArc(_objectPosition, Vector3.up, Vector3.forward, 360, radius);

            
            var viewAngle1 = DirectionFromAngle(_objectAngles.y, angle / 2);
            var viewAngle2 = DirectionFromAngle(_objectAngles.y, angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(_objectPosition, _objectPosition + viewAngle1 * radius);
            Handles.DrawLine(_objectPosition, _objectPosition + viewAngle2 * radius);

            if (isCustomerFound)
            {
                Handles.color = Color.blue;
                Handles.DrawLine(_objectPosition, foundObject.transform.position);
            }
        }
        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }

    public interface IBaseFieldOfView
    {
        bool IsFieldOfViewActive { get; }
        void ToggleFieldOfView(bool isActivated);
    }

    public class BaseFieldOfView : MonoBehaviour, IBaseFieldOfView
    {
        public float radius;
        [Range(0,360)]
        public float angle;
        private float delay = 0.2f;

        public GameObject foundObject;

        public LayerMask customerLayerMask;
        public LayerMask obstructionMask;

        public bool isCustomerFound;
        public bool IsFieldOfViewActive => _mIsFieldOfViewActive;
        public void ToggleFieldOfView(bool isActivated)
        {
            _mIsFieldOfViewActive = isActivated;
        }
        
        private bool _mIsFieldOfViewActive = false;

        protected virtual void Start()
        {
            _mIsFieldOfViewActive = true;
            StartCoroutine(FoVRoutine());
        }

        protected IEnumerator FoVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(delay);
            while (_mIsFieldOfViewActive)
            {
                yield return wait;
                ManageFieldOfView();
            }
        }

        protected void ManageFieldOfView()
        {
            Collider[] customersInRadius = Physics.OverlapSphere(transform.position, radius, customerLayerMask);

            CalculateFieldOfView();
            if (customersInRadius.Length != 0)
            {
                foreach (var customerCollider in customersInRadius)
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

        protected void CalculateFieldOfView()
        {
            
            Debug.Log("Field of view Angle is");
        }
    }
}