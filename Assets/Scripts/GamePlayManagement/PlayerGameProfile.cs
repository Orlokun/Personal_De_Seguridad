using System;
using System.Collections.Generic;
using DataUnits.GameCatalogues;
using DataUnits.GameRequests;
using DataUnits.ItemScriptableObjects;
using DialogueSystem;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;
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
            ICalendarModule calendarManager, ILifestyleModule lifeStyleModule, IPlayerGameStatusModule statusModule, IRequestsModuleManager requestModuleManager)
        {
            _mGameCreationDate = DateTime.Now;
            _mGameId = Guid.NewGuid();
            
            _itemSuppliersModule = itemSuppliersModule;
            _itemSuppliersModule.SetProfile(this);
            
            _jobsSourcesModule = jobsSourcesModule;
            _jobsSourcesModule.SetProfile(this);
            
            _calendarModule = calendarManager;
            _calendarModule.SetProfile(this);
            
            _gameStatusModule = statusModule;
            _gameStatusModule.SetProfile(this);
            
            _lifeStyleModule = lifeStyleModule;
            _lifeStyleModule.SetProfile(this);
            
            _mRequestModuleManager = requestModuleManager;
            _mRequestModuleManager.SetProfile(this);
            
        }
        private void PlayerLostResetData()
        {
            _itemSuppliersModule.PlayerLostResetData();
            _jobsSourcesModule.PlayerLostResetData();
            _calendarModule.PlayerLostResetData();
            _lifeStyleModule.PlayerLostResetData();
            _gameStatusModule.PlayerLostResetData();
            _mRequestModuleManager.PlayerLostResetData();
        }
        //Main Data Modules
        private IItemSuppliersModule _itemSuppliersModule;
        private IJobsSourcesModule _jobsSourcesModule;
        private ICalendarModule _calendarModule;
        private ILifestyleModule _lifeStyleModule;
        private IPlayerGameStatusModule _gameStatusModule;
        
        //Members
        private DateTime _mGameCreationDate;
        private Guid _mGameId;
        private readonly IRequestsModuleManager _mRequestModuleManager;


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
            return _calendarModule;
        }
        public ILifestyleModule GetLifestyleModule()
        {
            return _lifeStyleModule;
        }

        public IPlayerGameStatusModule GetStatusModule()
        {
            return _gameStatusModule;
        }
        public IRequestsModuleManager GetRequestsModuleManager()
        {
            return _mRequestModuleManager;
        }
        
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
            var activeProviders = GetActiveItemSuppliersModule().ActiveProviderObjects;
            foreach (var itemSupplier in activeProviders)
            {
                foreach (var suppliersItem in itemsInCatalogue[itemSupplier.Value.BitSupplierId])
                {
                    if (suppliersItem.UnlockPoints <= _gameStatusModule.MPlayerStatus)
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
        }

        public IWorkDayObject GetCurrentWorkday()
        {
            return _calendarModule.GetCurrentWorkDayObject();
        }

        public void PlayerLost(EndingTypes organSale)
        {
            _gameStatusModule.PlayerLostGame(organSale);
            PlayerLostResetData();
        }
    }
}