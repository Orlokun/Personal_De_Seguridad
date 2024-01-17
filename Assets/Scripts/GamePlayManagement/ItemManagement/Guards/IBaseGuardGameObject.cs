using GamePlayManagement.Players_NPC;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IBaseGuardGameObject : IHasFieldOfView, IBaseCharacterInScene
    {
        public void SetInPlacementStatus(bool inPlacement);
    }
}