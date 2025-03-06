using System;
using System.Linq;
using DataUnits.GameCatalogues;
using DialogueSystem;
using GameDirection.ComplianceDataManagement;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.GameRequests.RequestsManager;
using GamePlayManagement.GameRequests.RewardsPenalties;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using UnityEngine;
using Utils;

namespace GamePlayManagement
{
    public class PlayerGameProfile : IPlayerGameProfile
    {
        //Profile constructor
        public PlayerGameProfile(IItemSuppliersModule itemSuppliersModule, IJobsSourcesModule jobsSourcesModule, 
            ICalendarModule mCalendarManager, ILifestyleModule lifeStyleModule, IPlayerGameStatusModule statusModule,
            IRequestsModuleManager requestModuleManager, IComplianceManager complianceManager, IPlayerInventoryModule inventoryModule)
        {
            _mGameCreationDate = DateTime.Now;
            _mGameId = Guid.NewGuid();
            
            _itemSuppliersModule = itemSuppliersModule;
            _itemSuppliersModule.SetProfile(this);
            
            _jobsSourcesModule = jobsSourcesModule;
            _jobsSourcesModule.SetProfile(this);
            
            _mCalendarModule = mCalendarManager;
            _mCalendarModule.SetProfile(this);
            
            _gameStatusModule = statusModule;
            _gameStatusModule.SetProfile(this);
            
            _lifeStyleModule = lifeStyleModule;
            _lifeStyleModule.SetProfile(this);
            
            _mRequestModuleManager = requestModuleManager;
            _mRequestModuleManager.SetProfile(this);

            _mComplianceManager = complianceManager;
            _mComplianceManager.SetProfile(this);

            _mInventoryModule = inventoryModule;
            _mInventoryModule.SetProfile(this);
        }
        private void PlayerLostResetData()
        {
            _itemSuppliersModule.PlayerLostResetData();
            _jobsSourcesModule.PlayerLostResetData();
            _mCalendarModule.PlayerLostResetData();
            _lifeStyleModule.PlayerLostResetData();
            _gameStatusModule.PlayerLostResetData();
            _mRequestModuleManager.PlayerLostResetData();
            _mInventoryModule.PlayerLostResetData();
        }
        //Main Data Modules
        private IItemSuppliersModule _itemSuppliersModule;
        private IJobsSourcesModule _jobsSourcesModule;
        private ICalendarModule _mCalendarModule;
        private ILifestyleModule _lifeStyleModule;
        private IPlayerGameStatusModule _gameStatusModule;
        private IPlayerInventoryModule _mInventoryModule;
        
        //Members
        private DateTime _mGameCreationDate;
        private Guid _mGameId;
        private readonly IRequestsModuleManager _mRequestModuleManager;
        private readonly IComplianceManager _mComplianceManager;


        //Public Fields
        public DateTime GameCreationDate => _mGameCreationDate;
        public Guid GameId => _mGameId;
        
        //Public Module Interfaces Fields
        public IItemSuppliersModule GetActiveItemSuppliersModule()
        {
            return _itemSuppliersModule;
        }
        public IJobsSourcesModule GetActiveJobsModule()
        {
            return _jobsSourcesModule;
        }
        public ICalendarModule GetProfileCalendar()
        {
            return _mCalendarModule;
        }
        public ILifestyleModule GetLifestyleModule()
        {
            return _lifeStyleModule;
        }

        public IPlayerGameStatusModule GetStatusModule()
        {
            return _gameStatusModule;
        }

        public IPlayerInventoryModule GetInventoryModule()
        {
            return _mInventoryModule;
        }

        public IRequestsModuleManager GetRequestsModuleManager()
        {
            return _mRequestModuleManager;
        }

        public IComplianceManager GetComplianceManager => _mComplianceManager;

        
        public int GameDifficulty => _gameStatusModule.GameDifficulty;

        #region UpdateProfileData
        public void UpdateProfileData()
        {
            UpdateJobSuppliersInProfile();
            UpdateItemSuppliersInProfile();
            UpdateItemsInProfile();
        }
        
        private void UpdateJobSuppliersInProfile()
        {
            var jobSuppliersInData = BaseJobsCatalogue.Instance.JobSuppliersInData;
            foreach (var jobSupplierObject in jobSuppliersInData)
            {
                if (BitOperator.IsActive(GetActiveJobsModule().MUnlockedJobSuppliers, (int)jobSupplierObject.JobSupplierData.JobSupplierBitId))
                {
                    GetActiveJobsModule().UnlockJobSupplier(jobSupplierObject.JobSupplierData.JobSupplierBitId);
                }
            }
        }
        private void UpdateItemSuppliersInProfile()
        {
            var itemSuppliersInData = BaseItemSuppliersCatalogue.Instance.GetItemSuppliersInData;
            foreach (var itemSupplier in itemSuppliersInData)
            {
                if (BitOperator.IsActive(GetActiveItemSuppliersModule().UnlockedItemSuppliers, (int)itemSupplier.ItemSupplierId))
                {
                    GetActiveItemSuppliersModule().UnlockSupplier(itemSupplier.ItemSupplierId);
                }
            }
        }
        private void UpdateItemsInProfile()
        {
            var itemsInCatalogue = ItemsDataController.Instance.ExistingBaseItemsInCatalogue;
            var activeProviders = GetActiveItemSuppliersModule().ActiveItemStores;
            foreach (var itemSupplier in activeProviders)
            {
                foreach (var suppliersItem in itemsInCatalogue[itemSupplier.Value.BitSupplierId])
                {
                    if (suppliersItem.UnlockPoints <= _gameStatusModule.MPlayerSeniority)
                    {
                        GetActiveItemSuppliersModule().UnlockItemInSupplier(itemSupplier.Key, suppliersItem.BitId);
                        Debug.Log($"Added Item {suppliersItem.ItemName} to Supplier {itemSupplier.Value.GetSupplierData.StoreName}");
                    }
                }
            }
        }

        #endregion

        public int GeneralOmniCredits
        {
            get => _gameStatusModule.PlayerOmniCredits;
        }
        public void UpdateDataEndOfDay()
        {
            _jobsSourcesModule.ProcessEndOfDay();
            _mComplianceManager.StartComplianceEndOfDayProcess(_mCalendarModule.CurrentDayBitId);
        }

        public IWorkDayObject GetCurrentWorkday()
        {
            return _mCalendarModule.GetCurrentWorkDayObject();
        }

        public void PlayerLost(EndingTypes organSale)
        {
            _gameStatusModule.PlayerLostGame(organSale);
            PlayerLostResetData();
        }

        public void AddFondnessToActiveSupplier(ITrustRewardData trustRewardData)
        {
            if(_jobsSourcesModule.ActiveJobObjects.Any(x=> x.Value.SpeakerIndex == trustRewardData.TrustGiverSpeakerId))
            {
                var supplier = _jobsSourcesModule.ActiveJobObjects.SingleOrDefault(x =>
                    x.Value.SpeakerIndex == trustRewardData.TrustGiverSpeakerId).Value;
                supplier.ReceiveFondness(trustRewardData.TrustAmount);
            }
        }
    }
}