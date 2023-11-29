using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using DialogueSystem;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;

namespace Utils
{
    public static class Factory
    {
        public static ITransportDataObject CreateTransportDataObject(TransportTypesId mTransportTypeId, string mRentName, int mRentPrice, int mUnlockLevel, int mSpecialCondition,
            int mRentEnergyBonus, int mRentSanityBonus)
        {
            return new TransportDataObject (mTransportTypeId, mRentName, mRentPrice, mUnlockLevel, mSpecialCondition,
                mRentEnergyBonus, mRentSanityBonus);
        }
        public static IFoodDataObject CreateFoodDataObject(FoodTypesId mFoodTypeId, string mFoodName, int mFoodPrice, int mUnlockLevel, int mSpecialCondition,
            int mFoodEnergyBonus, int mFoodSanityBonus)
        {
            return new FoodDataObject (mFoodTypeId, mFoodName, mFoodPrice, mUnlockLevel, mSpecialCondition,
                mFoodEnergyBonus, mFoodSanityBonus);
        }
        
        public static IRentDataObject CreateRentDataObject(RentTypesId mRentId, string mRentName, int mRentPrice, int mUnlockLevel, int mSpecialCondition,
            int mRentEnergyBonus, int mRentSanityBonus)
        {
            return new RentDataObject (mRentId, mRentName, mRentPrice, mUnlockLevel, mSpecialCondition,
                mRentEnergyBonus, mRentSanityBonus);
        }
        public static WorkDayObject CreateWorkday(DayBitId dayBitId)
        {
            return new WorkDayObject(dayBitId);
        }
      
        public static DialogueCameraMan CreateCameraMan()
        {
            return new DialogueCameraMan();
        }
        public static ItemSupplierShop CreateItemStoreSupplier(BitItemSupplier itemSupplier, IItemsDataController itemDataController,IBaseItemSuppliersCatalogue suppliersCatalogue)
        {
            return new ItemSupplierShop(itemSupplier, itemDataController, suppliersCatalogue);
        }
        
        public static PlayerGameProfile CreatePlayerGameProfile(IItemSuppliersModule itemSuppliersModule, IJobsSourcesModule jobsModule, 
            ICalendarModule calendarManager, ILifestyleModule lifeStyleModule)
        {
            return new PlayerGameProfile(itemSuppliersModule, jobsModule, calendarManager, lifeStyleModule);
        }

        /// <summary>
        /// Player Profile Modules
        /// </summary>
        /// <param name="itemDataController"></param>
        /// <param name="suppliersCatalogue"></param>
        /// <returns></returns>
        public static IItemSuppliersModule CreateItemSuppliersModule(IItemsDataController itemDataController, IBaseItemSuppliersCatalogue suppliersCatalogue)
        {
            return new ItemSuppliersModule(itemDataController, suppliersCatalogue);
        }
        public static IJobsSourcesModule CreateJobSourcesModule(IBaseJobsCatalogue jobsCatalogue)
        {
            return new JobsSourcesModule(jobsCatalogue);
        }
        public static ILifestyleModule CreateLifestyleModule(IRentValuesCatalogue rentCatalogue, IFoodValuesCatalogue foodCatalogue, ITransportValuesCatalogue transportsCatalogue)
        {
            return new LifestyleModule(rentCatalogue, foodCatalogue, transportsCatalogue);
        }
        public static CalendarModule CreateCalendarModule()
        {
            return new CalendarModule(DayBitId.DayOne, PartOfDay.EarlyMorning);
        }
    }
}