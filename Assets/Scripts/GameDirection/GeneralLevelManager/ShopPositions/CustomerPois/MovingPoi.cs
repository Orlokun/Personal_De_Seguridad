using UnityEngine;

namespace GameDirection.GeneralLevelManager.ShopPositions.CustomerPois
{
    public class MovingPoi : ShopPoiObject, IMovingPointOfInterest
    {
        private Vector3 _mStartPosition;
        private Vector3 _mSecondPosition;

        public Vector3 SecondPosition => _mSecondPosition;

        
        private Vector3 currentTargetPosition;
        
        [SerializeField] private Transform secondPositionObject;

        private void Awake()
        {
            _mStartPosition = transform.position;
            _mSecondPosition = secondPositionObject.position;

            currentTargetPosition = _mSecondPosition;
        }
        

        private void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, .03f);
            if (Vector3.Distance(transform.position, currentTargetPosition) <= .1f)
            {
                currentTargetPosition = currentTargetPosition == _mSecondPosition ? _mStartPosition : _mSecondPosition;
            }
        }
    }
}