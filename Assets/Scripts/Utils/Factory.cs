using System;
using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using DialogueSystem;
using ExternalAssets._3DFOV.Scripts;
using GameDirection;
using GameDirection.ComplianceDataManagement;
using GameDirection.DayLevelSceneManagers;
using GameDirection.GeneralLevelManager.ShopPositions;
using GameDirection.NewsManagement;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ComplianceSystem;
using GamePlayManagement.GameRequests;
using GamePlayManagement.GameRequests.RequestsManager;
using GamePlayManagement.ItemManagement.Guards;
using GamePlayManagement.ItemPlacement;
using GamePlayManagement.ItemPlacement.PlacementManagement;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
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

        public static IGuardRouteModule CreateGuardSystemModule()
        {
            return new GuardRouteModule();
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
        public static ICustomerInstanceData CreateCustomerInstanceData(ICustomersInSceneManager sceneManager, Guid newId, IStoreEntrancePosition randomPositionData, ICustomerTypeData customerType)
        {
            return new CustomerInstanceData(sceneManager, newId, randomPositionData, customerType);
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
            ICalendarModule calendarManager, ILifestyleModule lifeStyleModule, IPlayerGameStatusModule statusModule, 
            IRequestsModuleManager requestModuleManager, IComplianceManager complianceManager, IPlayerInventoryModule inventoryModule)
        {
            return new PlayerGameProfile(itemSuppliersModule, jobsModule, calendarManager, lifeStyleModule, statusModule,
                requestModuleManager, complianceManager, inventoryModule);
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
        
        public static IPlayerInventoryModule CreateInventoryModule()
        {
            return new PlayerInventoryModule();
        }

        public static INewsNarrativeDirector CreateNewsNarrativeDirector()
        {
            return new NewsNarrativeDirector();
        }

        public static IMetaGameDirector CreateMetaGameDirectory()
        {
            return new MetaGameDirector();
        }

        #region Game Requests
        public static IGameRequest CreateGameRequest(int speakerId, int reqId, string reqTitle, string reqDescription, RequirementActionType requestType, 
            RequirementObjectType requiredObjectType, RequirementLogicEvaluator requirementLogicEvaluator, 
            RequirementConsideredParameter requestParameterType, string[] requestParameterValue, int quantity, string[] rewards, string[] punishment,
            DayBitId targetDayId, PartOfDay targetPartOfDay)
        {
            switch (requestType)
            {
                case RequirementActionType.Hire:
                    return CreateHireGameRequest(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, requestParameterValue, quantity, rewards, punishment, targetDayId, targetPartOfDay);
                case RequirementActionType.Use:
                    return CreateGameUseRequest(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, requestParameterValue, quantity, rewards, punishment, targetDayId, targetPartOfDay);
                case RequirementActionType.NotUse:
                    return CreateGameUseRequest(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, requestParameterValue, quantity, rewards, punishment, targetDayId, targetPartOfDay);
                case RequirementActionType.Trap:
                    return new GameRequest(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay);
                default:
                    return new GameRequest(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay);
            }
        }
            
        private static IHireGameRequest CreateHireGameRequest(int speakerId, int reqId, string reqTitle, string reqDescription,
            RequirementActionType requestType, RequirementObjectType requiredObjectType, RequirementLogicEvaluator requirementLogicEvaluator, 
            RequirementConsideredParameter requestParameterType, string[] requestParameterValue, int quantity, string[] rewards, string[] punishment,
            DayBitId targetDayId, PartOfDay targetPartOfDay)
        {
            int intSupplierId = int.Parse(requestParameterValue[0]);
            JobSupplierBitId jobSupplierBitId = (JobSupplierBitId)intSupplierId;
            return new HireGameRequest(speakerId, reqId, reqTitle, reqDescription, requestType, requiredObjectType, requirementLogicEvaluator,
                requestParameterType, jobSupplierBitId, quantity, rewards, punishment, targetDayId, targetPartOfDay);
        }

        private static IGameRequest CreateGameUseRequest(int speakerId, 
            int reqId, 
            string reqTitle,
            string reqDescription,
            RequirementActionType requestType, 
            RequirementObjectType requiredObjectType,
            RequirementLogicEvaluator requirementLogicEvaluator,
            RequirementConsideredParameter requestParameterType, 
            string[] requestParameterValue, 
            int quantity, 
            string[] rewards, 
            string[] punishment, 
            DayBitId targetDayId, 
            PartOfDay targetPartOfDay)
        {
            switch (requestParameterType)
            {
                case RequirementConsideredParameter.Origin:
                    return new UseItemByOriginRequest(speakerId, reqId, reqTitle, reqDescription, requestType,
                        requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay, requestParameterValue);
                case RequirementConsideredParameter.BaseType:
                    return new UseItemByBaseTypeRequest(speakerId, reqId, reqTitle, reqDescription, requestType,
                        requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay, requestParameterValue);
                case RequirementConsideredParameter.Quality:
                    return new UseItemByQualityRequest(speakerId, reqId, reqTitle, reqDescription, requestType,
                        requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay, requestParameterValue);
                case RequirementConsideredParameter.ItemSupplier:
                    return new UseItemBySupplier(speakerId, reqId, reqTitle, reqDescription, requestType,
                        requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay,
                        requestParameterValue);
                case RequirementConsideredParameter.ItemValue:
                    return new UseExactItemRequest(speakerId, reqId, reqTitle, reqDescription, requestType,
                        requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay,
                        requestParameterValue);
                case RequirementConsideredParameter.Variable:
                    return new UseExactItemRequest(speakerId, reqId, reqTitle, reqDescription, requestType,
                        requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay,
                        requestParameterValue);
                default:
                    return new GameRequest(speakerId, reqId, reqTitle, reqDescription, requestType,
                        requiredObjectType, requirementLogicEvaluator,
                        requestParameterType, quantity, rewards, punishment, targetDayId, targetPartOfDay);
            }
        }
        
        #endregion
        public static IComplianceObject CreateComplianceObject(int complianceId, DayBitId startDayId, DayBitId endDayId,
            bool needsUnlock,
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType,
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue,
            string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description,
            RequirementLogicEvaluator complianceLogic)
        {
            switch (actionType)
            {
                case ComplianceActionType.Use:
                    return CreateComplianceUseObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, 
                        actionType, objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
                case ComplianceActionType.NotUse:
                    return new ComplianceUseObjectByOrigin(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, 
                        actionType, objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
                case ComplianceActionType.KickOut:
                    return new ComplianceKickOut(complianceId, startDayId, endDayId, needsUnlock, motivationLvl,
                        actionType, objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues,
                        penaltyValues, title, subtitle, description, complianceLogic);
                case ComplianceActionType.Retain:
                    return new ComplianceRetainObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl,
                        actionType, objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues,
                        penaltyValues, title,subtitle, description, complianceLogic);
                default:
                    return new ComplianceObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, 
                        actionType, objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
            }
        }
        
        private static IComplianceObject CreateComplianceUseObject(int complianceId, DayBitId startDayId,
            DayBitId endDayId, bool needsUnlock,
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType,
            RequirementConsideredParameter consideredParameter, string[] complianceReqValues, int toleranceValue,
            string[] rewardValues, string[] penaltyValues,
            string title, string subtitle, string description, RequirementLogicEvaluator complianceLogic)
        {
            switch (consideredParameter)
            {
                case RequirementConsideredParameter.None:
                    return EvaluateComplianceType(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
                case RequirementConsideredParameter.Origin:
                    return new ComplianceUseObjectByOrigin(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
                case RequirementConsideredParameter.BaseType:
                    return new ComplianceUseObjectByBaseType(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
                case RequirementConsideredParameter.Quality:
                    return new ComplianceUseObjectByQuality(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
                case RequirementConsideredParameter.ItemSupplier:

                case RequirementConsideredParameter.ItemValue:

                case RequirementConsideredParameter.Variable:
                    return new ComplianceObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
                default:
                    return new ComplianceObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl, actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues, penaltyValues, title, subtitle, description, complianceLogic);
            }
        }

        private static IComplianceObject EvaluateComplianceType(int complianceId, DayBitId startDayId, DayBitId endDayId, bool needsUnlock, 
            ComplianceMotivationalLevel motivationLvl, ComplianceActionType actionType, ComplianceObjectType objectType, RequirementConsideredParameter consideredParameter,
            string[] complianceReqValues, int toleranceValue, string[] rewardValues, string[] penaltyValues, string title, string subtitle, string description, 
            RequirementLogicEvaluator complianceLogic)
        {
            switch (objectType)
            {
                case ComplianceObjectType.Radio:
                    return new ComplianceObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl,
                        actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues,
                        penaltyValues, title, subtitle, description, complianceLogic);
                case ComplianceObjectType.Smoke:
                    return new ComplianceObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl,
                        actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues,
                        penaltyValues, title, subtitle, description, complianceLogic);
                case ComplianceObjectType.DoorLock:
                    return new ComplianceObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl,
                        actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues,
                        penaltyValues, title, subtitle, description, complianceLogic);
                default:
                    Debug.LogWarning("[Factory.EvaluateComplianceType] Compliance object type must be valid");
                    return new ComplianceObject(complianceId, startDayId, endDayId, needsUnlock, motivationLvl,
                        actionType,
                        objectType, consideredParameter, complianceReqValues, toleranceValue, rewardValues,
                        penaltyValues, title, subtitle, description, complianceLogic);
            }
        }

        public static IBaseTutorialDialogueData CreateTutorialDialogueData()
        {
            return new BaseTutorialDialogueData();
        }

        public static IRequestsModuleManager CreateRequestsModuleManager()
        {
            return new RequestsModuleManager();
        }


        public static IComplianceManager CreateComplianceManager()
        {
            return new ComplianceManager();
        }


        public static IIntroSceneOperator CreateIntroSceneOperator()
        {
            return new DayZeroIntroScene();
        }
    }


}