using Utils;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IGuardRouteModule : IInitializeWithArg1<IShopInspectionPosition>
    {
        public IShopInspectionPosition GetStartPosition { get; }
        public void SetNewCurrentPosition(IShopInspectionPosition newPosition);
        public IShopInspectionPosition GetCurrentPosition { get; }
    }
}