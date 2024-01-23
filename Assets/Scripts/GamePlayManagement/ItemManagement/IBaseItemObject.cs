using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.ItemManagement
{
    public interface IBaseItemObject
    {
        public bool IsInPlacement { get; }
        public void SetInPlacementStatus(bool inPlacement);
        public void InitializeItem(IItemObject itemObjectData);
    }
}