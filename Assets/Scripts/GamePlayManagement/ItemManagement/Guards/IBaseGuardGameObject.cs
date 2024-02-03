using DataUnits.ItemScriptableObjects;
using GamePlayManagement.Players_NPC;
using UnityEngine;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IBaseInspectionObject : IBaseCharacterInScene
    {
        public IShopInspectionPosition CurrentInspectionPosition { get; }
        public Vector3 CurrentManualInspectionPosition { get; }
        public void SetGuardDestination(Vector3 destination);
        public void IdleInspection();
    }
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