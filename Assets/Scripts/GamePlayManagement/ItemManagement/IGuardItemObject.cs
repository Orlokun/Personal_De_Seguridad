using GamePlayManagement.ItemManagement.Guards;

namespace GamePlayManagement.ItemManagement
{
    public interface IGuardItemObject : IBaseItemObject
    {
        public IBaseGuardGameController GuardData { get; }
    }
}