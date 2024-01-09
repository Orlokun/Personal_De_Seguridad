public interface IBaseItemObject : IInteractiveClickableObject
{
    public bool IsInPlacement { get; }
    public void SetInPlacementStatus(bool inPlacement);
}