using DataUnits.ItemScriptableObjects;
using UnityEngine;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IBaseGuardGameObject : IHasFieldOfView, IBaseInspectionObject
    {
        public void SetInPlacementStatus(bool inPlacement);
        public Transform GunParentTransform { get; }
        public void ApplyWeapon(GameObject itemObject, IItemObject appliedWeapon);
        public void DestroyWeapon();
        public void Initialize(IItemObject itemObjectData);
        public void StartBehaviorTree();
    }
}