using System;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using UnityEngine.AI;

namespace GamePlayManagement.Players_NPC
{
    public interface IBaseCharacterInScene : ICharacterNavMesh, IBaseCharacterMovementStatus
    {
        public Guid CharacterId { get; }
    }
        
    public interface IBaseCharacterMovementStatus
    {
        public BaseCharacterMovementStatus CharacterMovementStatus { get; }
    }

    public interface ICharacterNavMesh
    {
        public NavMeshAgent GetNavMeshAgent { get; }
        public void ToggleNavMesh(bool isActive);
    }
}