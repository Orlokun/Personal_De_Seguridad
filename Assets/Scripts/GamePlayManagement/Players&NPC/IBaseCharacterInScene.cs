using System;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.Players_NPC.Animations.Interfaces;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;
using UnityEngine.AI;

namespace GamePlayManagement.Players_NPC
{
    public interface IBaseCharacterInScene : ICharacterNavMesh
    {
        public Guid CharacterId { get; }
        public event BaseCharacterInScene.ReachDestination WalkingDestinationReached;
        public void ChangeMovementState<T>() where T : IMovementState;
        public void ChangeAttitudeState<T>() where T : IAttitudeState;
        public IBaseAnimatedAgent BaseAnimator { get; }
        public IShopPositionsManager GetPositionsManager { get; }
    }
        
    public interface ICharacterNavMesh
    {
        public NavMeshAgent GetNavMeshAgent { get; }
        public void ToggleNavMesh(bool isActive);
    }
}