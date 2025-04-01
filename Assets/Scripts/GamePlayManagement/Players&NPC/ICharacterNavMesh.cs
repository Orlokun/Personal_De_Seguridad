using UnityEngine.AI;

namespace GamePlayManagement.Players_NPC
{
    public interface ICharacterNavMesh
    {
        public NavMeshAgent GetNavMeshAgent { get; }
        public void ToggleNavMesh(bool isActive);
    }
}