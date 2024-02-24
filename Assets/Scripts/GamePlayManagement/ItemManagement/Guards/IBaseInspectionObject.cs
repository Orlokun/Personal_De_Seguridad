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
}