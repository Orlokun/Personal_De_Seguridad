using GamePlayManagement.ItemManagement;
using UnityEngine;

namespace GamePlayManagement.ItemPlacement.PlacementManagement
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
            IBasePlacementPosition weaponPositionData;
            base.SetCurrentMousePosition();
            weaponPositionData = GetPlacementPoint(MousePosition);
            if (weaponPositionData == null)
            {
                CurrentPlacedObject.SetActive(false);
                return;
            }
            _currentWeaponPosition = (IWeaponPlacementPosition)weaponPositionData;
            CurrentPlacedObject.transform.position = _currentWeaponPosition.ItemPosition;
            if (!CurrentPlacedObject.activeInHierarchy)
            {
                CurrentPlacedObject.SetActive(true);
            }
        }
        protected override void SetNewObjectPosition(GameObject gObject)
        {
            gObject.transform.position = Vector3.zero;
            gObject.transform.rotation = new Quaternion();
            Debug.Log("WeaponPlacementManager needs no Set Object Position");
        }

        protected override void RotateObjectPreview()
        {
            
        }
        protected override void CreateObjectInPlace()
        {
            base.CreateObjectInPlace();
            _currentWeaponPosition.GuardObject.GuardData.ApplyWeapon(LastInstantiatedGameObject, CurrentItemData);
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