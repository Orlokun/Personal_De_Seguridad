using GameManagement.ProfileDataModules.ItemStores.Stores;

namespace GameManagement.ProfileDataModules.ItemStores.StoreInterfaces
{
    public interface IGeneralItemSources
    {
        public IGuardSourcesModule GuardSourcesModule { get; }
        public ICameraSourcesModule CameraSourcesModule { get; }
        public IWeaponsSourcesModule WeaponSourcesModule { get; }
        public ITrapSourcesModule TrapSourcesModule { get; }
        public IOtherItemsSourcesModule OtherItemsSourcesModule { get; }
    }
}