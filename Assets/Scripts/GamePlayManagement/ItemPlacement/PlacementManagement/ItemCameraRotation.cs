using UnityEngine;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
{
    public class ItemCameraRotation : MonoBehaviour, IItemCameraRotation
    {
        private Vector3 stablePosition;
        private bool isStatic;

        // Update is called once per frame
        void Update()
        {
            if (isStatic)
            {
                transform.position = stablePosition;
            }
        }

        public void ToggleComponentActive(bool isActive)
        {
            isStatic = isActive;
        }

        public void SetNewPosition(Vector3 newPosition)
        {
            stablePosition = newPosition;
        }
    }
}