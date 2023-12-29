using System;
using System.Collections.Generic;
using DataUnits.GameCatalogues;
using DataUnits.ItemScriptableObjects;
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
            ICalendarModule calendarManager, ILifestyleModule lifeStyleModule)
        {
            _totalOmniCredits = 20000;
            _socialStatus = 10;
            _health = 20;
            
            _mGameCreationDate = DateTime.Now;
            _mGameId = Guid.NewGuid();
            
            _itemSuppliersModule = itemSuppliersModule;
            _itemSuppliersModule.SetProfile(this);
            
            _jobsSourcesModule = jobsSourcesModule;
            _jobsSourcesModule.SetProfile(this);
            
            _calendarModule = calendarManager;
            _calendarModule.SetProfile(this);

            _lifeStyleModule = lifeStyleModule;
            _lifeStyleModule.SetProfile(this);
        }
        
        //Main Data Modules
        private IItemSuppliersModule _itemSuppliersModule;
        private IJobsSourcesModule _jobsSourcesModule;
        private ICalendarModule _calendarModule;
        private ILifestyleModule _lifeStyleModule;
        
        //Members
        private DateTime _mGameCreationDate;
        private Guid _mGameId;
        private int _totalOmniCredits;
        [Range(-100,100)]
        private int _socialStatus;
        [Range(-100,100)]
        private int _health;
        
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
                if (jobSupplierObject.StoreUnlockPoints <= GeneralOmniCredits)
                {
                    GetActiveJobsModule().UnlockJobSupplier(jobSupplierObject.JobSupplierBitId);
                }
            }
        }
        private void UpdateItemSuppliersInProfile()
        {
            var itemSuppliersInData = BaseItemSuppliersCatalogue.Instance.GetItemSuppliersInData;
            foreach (var itemSupplier in itemSuppliersInData)
            {
                if (itemSupplier.StoreUnlockPoints <= GeneralOmniCredits)
                {
                    GetActiveItemSuppliersModule().UnlockSupplier(itemSupplier.ItemSupplierId);
                }
            }
        }
        private void UpdateItemsInProfile()
        {
            var itemsInCatalogue = ItemsDataController.Instance.ExistingBaseItemsInCatalogue;
            UpdateItemsSpecialStats(itemsInCatalogue);
            var activeProviders = GetActiveItemSuppliersModule().ActiveProviderObjects;
            foreach (var itemSupplier in activeProviders)
            {
                foreach (var suppliersItem in itemsInCatalogue[itemSupplier.Value.BitSupplierId])
                {
                    if (suppliersItem.UnlockPoints <= GeneralOmniCredits)
                    {
                        GetActiveItemSuppliersModule().UnlockItemInSupplier(itemSupplier.Key, suppliersItem.BitId);
                        Debug.Log($"Added Item {suppliersItem.ItemName} to Supplier {itemSupplier.Value.GetSupplierData.StoreName}");
                    }
                }
            }
        }
        private void UpdateItemsSpecialStats(Dictionary<BitItemSupplier, List<IItemObject>> itemsInData)
        {
            foreach (var itemSupplier in itemsInData)
            {
                foreach (var itemObject in itemSupplier.Value)
                {
                    var specialStats = ItemsDataController.Instance.GetItemStats(itemSupplier.Key, itemObject.ItemType ,itemObject.BitId);
                    itemObject.SetItemSpecialStats(specialStats);
                }
            }
        }
        #endregion

        public int GeneralOmniCredits
        {
            get => _totalOmniCredits;
            set => _totalOmniCredits = value;
        }
        public void UpdateDataEndOfDay()
        {
            _jobsSourcesModule.CheckFinishDay();
        }
    }
}