using DataUnits.ItemScriptableObjects;
using GamePlayManagement.Players_NPC;
using UnityEngine;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IBaseInspectionObject
    {
        public IShopInspectionPosition CurrentInspectionPosition { get; }
    }
    public interface IBaseGuardGameObject : IHasFieldOfView, IBaseCharacterInScene, IBaseInspectionObject
    {
        public void SetInPlacementStatus(bool inPlacement);
        public Transform GunParentTransform { get; }
        public void ApplyWeapon(GameObject itemObject, IItemObject appliedWeapon);
        public void DestroyWeapon();
        public void Initialize(IItemObject itemObjectData);
        public void StartBehaviorTree();
    }
}