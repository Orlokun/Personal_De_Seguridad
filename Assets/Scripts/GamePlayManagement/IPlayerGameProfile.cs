using GameManagement.ProfileDataModules.ItemStores;
using GameManagement.ProfileDataModules.ItemStores.StoreInterfaces;
using GamePlayManagement.ProfileDataModules.ItemStores;
using UI;

namespace GameManagement
{
    public interface IPlayerGameProfile
    {
        public IItemSourceModule GetActiveItemsInModule(BitItemType bitIndex);
        public IGuardSourcesModule GuardSourcesModule { get; }
        public ICameraSourcesModule CameraSourcesModule { get; }
        public IWeaponsSourcesModule WeaponSourcesModule { get; }
        public ITrapSourcesModule TrapSourcesModule { get; }
        public IOtherItemsSourcesModule OtherItemsSourcesModule { get; }
    }
}