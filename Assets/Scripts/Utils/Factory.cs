using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using DialogueSystem;
using GameDirection.DayLevelSceneManagers;
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
        public static SupplierCallDialogueDataObject CreateSupplierCallDialogueDataObject(int callDialogueIndex, int jobDay, int callHour, int callMinute, PartOfDay partOfDay)
        {
            return new SupplierCallDialogueDataObject(callDialogueIndex, jobDay, callHour,
                callMinute, partOfDay);
        }
        
        public static ILevelDayManager CreateLevelDayManager(DayBitId dayId)
        {
            switch (dayId)
            {
                case DayBitId.Day_01:
                    return new DayOneLevelSceneManagement();
                case DayBitId.Day_02:
                    return new DayTwoLevelSceneManagement();
                case DayBitId.Day_03:
                    return new DayThreeLevelSceneManagement();
                case DayBitId.Day_04:
                    return new DayFourLevelSceneManagement();
                case DayBitId.Day_05:
                    return new DayFiveLevelSceneManagement();
                case DayBitId.Day_06:
                    return new DaySixLevelSceneManagement();
                case DayBitId.Day_07:
                    return new DaySevenLevelSceneManagement();
                case DayBitId.Day_08:
                    return new DayEightLevelSceneManagement();
                case DayBitId.Day_09:
                    return new DayNineLevelSceneManagement();
                case DayBitId.Day_10:
                    return new DayTenLevelSceneManagement();
                case DayBitId.Day_11:
                    return new DayElevenLevelSceneManagement();
                case DayBitId.Day_12:
                    return new DayTwelveLevelSceneManagement();
                case DayBitId.Day_13:
                    return new DayThirteenLevelSceneManagement();
                case DayBitId.Day_14:
                    return new DayFourteenLevelSceneManagement();
                case DayBitId.Day_15:
                    return new DayFifteenLevelSceneManagement();
                default:
                    return null;
            }
        }
        //Dialogue Object section
        
        /// <summary>
        /// End Of Day Section
        /// </summary>

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

        public static IItemSupplierShop CreateItemStoreSupplier(BitItemSupplier itemSupplier, IItemsDataController itemDataController,IBaseItemSuppliersCatalogue suppliersCatalogue)
        {
            return new ItemSupplierShop(itemSupplier, itemDataController, suppliersCatalogue);
        }

        /// <summary>
        /// Player Profile Modules
        /// </summary>
        public static PlayerGameProfile CreatePlayerGameProfile(IItemSuppliersModule itemSuppliersModule, IJobsSourcesModule jobsModule, 
            ICalendarModule calendarManager, ILifestyleModule lifeStyleModule)
        {
            return new PlayerGameProfile(itemSuppliersModule, jobsModule, calendarManager, lifeStyleModule);
        }
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
        public static CalendarModule CreateCalendarModule(IClockManagement clockManagement)
        {
            return new CalendarModule(DayBitId.Day_01, PartOfDay.EarlyMorning, clockManagement);
        }
    }
}