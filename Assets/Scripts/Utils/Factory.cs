using System;
using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using DialogueSystem;
using ExternalAssets._3DFOV.Scripts;
using GameDirection;
using GameDirection.DayLevelSceneManagers;
using GameDirection.GeneralLevelManager;
using GameDirection.GeneralLevelManager.ShopPositions;
using GameDirection.NewsManagement;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ItemManagement;
using GamePlayManagement.ItemManagement.Guards;
using GamePlayManagement.ItemPlacement;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using GamePlayManagement.Players_NPC;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;
using UnityEngine;

namespace Utils
{
    public static class Factory
    {
        public static GuardRouteSystemModule CreateGuardSystemModule()
        {
            return new GuardRouteSystemModule();
        }
        
        public static IGuardStatusModule CreateGuardStatusModule(IBaseGuardGameObject baseGuard)
        {
            return new GuardStatusModule(baseGuard);
        }
        public static IFieldOfViewItemModule CreateFieldOfViewItemModule(DrawFoVLines myDrawFieldOfView, FieldOfView3D my3dFieldOfView)
        {
            return new FieldOfViewItemModule(myDrawFieldOfView, my3dFieldOfView);
        }
        public static IStoreEntrancePosition CreateStartPosition(Transform instantiationPos, Transform entrancePos)
        {
            return new StoreEntrancePosition(instantiationPos,entrancePos);
        }
        public static ICustomersInstantiationFlowData CreateCustomersInSceneManagerData(JobSupplierBitId jobId, int gameDifficultyLvl, int maxClients, string clientsPrefabsPath, int[] timeRange)
        {
            return new CustomersInstantiationFlowData(jobId, gameDifficultyLvl, maxClients, clientsPrefabsPath, timeRange);
        }
        public static FloorPlacementPosition CreateFloorPlacementPosition(Vector3 position)
        {
            return new FloorPlacementPosition(position);
        }
        public static CameraPlacementPosition CreateCameraPlacementPosition(Guid id, Vector3 position, string positionName)
        {
            return new CameraPlacementPosition(id, position, positionName);
        }
        public static StoreProductObjectData CreateStoreProductObject(ProductsLevelEden id, string name, int type, int quantity, int price, int hideChances, int tempting, int punishment,
            string prefabName, string productBrand, string productSpriteName,string productDescription)
        {
            return new StoreProductObjectData(id, name, type, quantity, price, hideChances, tempting, punishment,
                 prefabName, productBrand, productSpriteName, productDescription);
        }
        public static BaseCustomerTypeData CreateBaseCustomerTypeData()
        {
            return new BaseCustomerTypeData();
        }
        public static IModularDialogueDataController CreateModularDialoguesDataController()
        {
            return new ModularDialogueDataController();
        }
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
            ICalendarModule calendarManager, ILifestyleModule lifeStyleModule, IPlayerGameStatusModule statusModule)
        {
            return new PlayerGameProfile(itemSuppliersModule, jobsModule, calendarManager, lifeStyleModule, statusModule);
        }
        public static IItemSuppliersModule CreateItemSuppliersModule(IItemsDataController itemDataController, IBaseItemSuppliersCatalogue suppliersCatalogue)
        {
            return new ItemSuppliersModule(itemDataController, suppliersCatalogue);
        }
        public static IJobsSourcesModule CreateJobSourcesModule(IBaseJobsCatalogue jobsCatalogue)
        {
            return new JobSourceModule(jobsCatalogue);
        }
        public static ILifestyleModule CreateLifestyleModule(IRentValuesCatalogue rentCatalogue, IFoodValuesCatalogue foodCatalogue, ITransportValuesCatalogue transportsCatalogue)
        {
            return new LifestyleModule(rentCatalogue, foodCatalogue, transportsCatalogue);
        }
        public static CalendarModule CreateCalendarModule(IClockManagement clockManagement)
        {
            return new CalendarModule(DayBitId.Day_01, PartOfDay.EarlyMorning, clockManagement);
        }

        public static PlayerGameStatusModule CreatePlayerStatusModule()
        {
            return new PlayerGameStatusModule();
        }

        public static INewsNarrativeDirector CreateNewsNarrativeDirector()
        {
            return new NewsNarrativeDirector();
        }

        public static IMetaGameDirector CreateMetaGameDirectory()
        {
            return new MetaGameDirector();
        }
    }
}