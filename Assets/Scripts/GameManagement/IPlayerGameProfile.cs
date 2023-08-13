using GameManagement.ProfileDataModules.ItemStores;
using GameManagement.ProfileDataModules.ItemStores.StoreInterfaces;
using UI;

namespace GameManagement
{
    public interface IPlayerGameProfile
    {
        public IItemSourceModule GetItemSourceWithIndex(BitItemType bitIndex);
        public IGuardSourcesModule GuardSourcesModule { get; }
        public ICameraSourcesModule CameraSourcesModule { get; }
        public IWeaponsSourcesModule WeaponSourcesModule { get; }
        public ITrapSourcesModule TrapSourcesModule { get; }
        public IOtherItemsSourcesModule OtherItemsSourcesModule { get; }
    }
}