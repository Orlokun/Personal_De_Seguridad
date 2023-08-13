using Utils;

namespace GameManagement.ProfileDataModules.ItemStores.StoreInterfaces
{
    public class GeneralItemSource : IGeneralItemSources
    {
        public IGuardSourcesModule GuardSourcesModule => _mGuardSourcesModule;
        public ICameraSourcesModule CameraSourcesModule => _mCameraSourcesModule;
        public IWeaponsSourcesModule WeaponSourcesModule => _mWeaponSourcesModule;
        public ITrapSourcesModule TrapSourcesModule => _mTrapSourcesModule;
        public IOtherItemsSourcesModule OtherItemsSourcesModule => _mOtherItemsSourcesModule;

        private IGuardSourcesModule _mGuardSourcesModule;
        private ICameraSourcesModule _mCameraSourcesModule;
        private IWeaponsSourcesModule _mWeaponSourcesModule;
        private ITrapSourcesModule _mTrapSourcesModule;
        private IOtherItemsSourcesModule _mOtherItemsSourcesModule;
    
        public GeneralItemSource()
        {
            _mGuardSourcesModule = Factory.CreateGuardSourcesModule();
            _mCameraSourcesModule = Factory.CreateCameraSourceModule();
            _mWeaponSourcesModule = Factory.CreateWeaponsSourceModule();
            _mTrapSourcesModule = Factory.CreateTrapSourceModule();
            _mOtherItemsSourcesModule = Factory.CreateOtherItemsSourceModule();
        }
    }
}