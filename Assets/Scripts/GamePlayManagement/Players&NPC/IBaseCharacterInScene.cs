using System;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using UnityEngine.AI;

namespace GamePlayManagement.Players_NPC
{
    public interface IBaseCharacterInScene : ICharacterNavMesh
    {
        public Guid CharacterId { get; }
    }
        
    public interface ICharacterNavMesh
    {
        public NavMeshAgent GetNavMeshAgent { get; }
        public void ToggleNavMesh(bool isActive);
    }
}