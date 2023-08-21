using DialogueSystem;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;

namespace Utils
{
    public static class Factory
    {
        public static DialogueCameraMan CreateCameraMan()
        {
            return new DialogueCameraMan();
        }
        public static ItemSupplierShop CreateItemStoreSupplier(BitItemSupplier itemSupplier)
        {
            return new ItemSupplierShop(itemSupplier);
        }
        
        public static PlayerGameProfile CreatePlayerGameProfile(IItemSuppliersModule itemSuppliersModule, IJobsSourcesModule jobsModule)
        {
            return new PlayerGameProfile(itemSuppliersModule, jobsModule);
        }

        public static IItemSuppliersModule CreateItemSuppliersModule()
        {
            return new ItemSuppliersModule();
        }
        public static IJobsSourcesModule CreateJobSourcesModule()
        {
            return new JobsSourcesModule();
        }
    }
}