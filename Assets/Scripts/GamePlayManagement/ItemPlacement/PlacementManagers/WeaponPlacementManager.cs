using GamePlayManagement.ItemManagement;
using UnityEngine;

namespace GamePlayManagement.ItemPlacement.PlacementManagers
{
    public class WeaponPlacementManager : BasePlacementManager
    {
        private static WeaponPlacementManager _instance;
        public static IBasePlacementManager Instance
        {
            get { return _instance; }
        }
        private float zDistance = 50f;
        #region Init
        protected override void Awake()
        {
            _instance = this;
            base.Awake();
        }
        #endregion

        private IWeaponPlacementPosition _currentWeaponPosition;
        
        protected override void SetCurrentMousePosition()
        {
            IBasePlacementPosition weaponPosition;
            base.SetCurrentMousePosition();
            weaponPosition = GetPlacementPoint(MousePosition);
            if (weaponPosition == null)
            {
                CurrentPlacedObject.SetActive(false);
                return;
            }
            _currentWeaponPosition = (IWeaponPlacementPosition)weaponPosition;
            CurrentPlacedObject.transform.position = _currentWeaponPosition.ItemPosition;
            if (!CurrentPlacedObject.activeInHierarchy)
            {
                CurrentPlacedObject.SetActive(true);
            }
        }
        protected override IBasePlacementPosition GetPlacementPoint(Vector3 mouseScreenPosition)
        {
            ConfirmCamera();
            var ray = MainCamera.ScreenPointToRay(mouseScreenPosition);
            RaycastHit hitInfo;
            Transform hitObject;
            if (Physics.Raycast(ray, out hitInfo, 500, targetLayerMask))
            {
                if (!hitInfo.collider.TryGetComponent<GuardItemObject>(out var guard))
                {
                    return null;
                }
                hitObject = hitInfo.transform;
                var weaponPlacementPosition = new WeaponPlacementPosition(guard);
                return weaponPlacementPosition;
            }
            return null;
        }
    }
}