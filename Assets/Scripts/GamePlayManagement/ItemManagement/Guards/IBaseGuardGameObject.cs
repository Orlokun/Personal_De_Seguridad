using DataUnits.ItemScriptableObjects;
using GamePlayManagement.Players_NPC;
using UnityEngine;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IBaseGuardGameObject : IHasFieldOfView, IBaseCharacterInScene
    {
        public void SetInPlacementStatus(bool inPlacement);
        public Transform GunParentTransform { get; }
        public void ApplyWeapon(GameObject itemObject, IItemObject appliedWeapon);
        public void DestroyWeapon();
    }
}