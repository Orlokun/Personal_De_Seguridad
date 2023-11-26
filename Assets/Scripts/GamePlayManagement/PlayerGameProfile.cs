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
            ICalendarManagement calendarManager)
        {
            _itemSuppliersModule = itemSuppliersModule;
            _jobsSourcesModule = jobsSourcesModule;
            _calendarModule = calendarManager;
            _mGameCreationDate = DateTime.Now;
            _mGameId = Guid.NewGuid();
            _generalXp = 3;
        }
        
        //Main Data Modules
        private IItemSuppliersModule _itemSuppliersModule;
        private IJobsSourcesModule _jobsSourcesModule;
        private ICalendarManagement _calendarModule;
        //Members
        private DateTime _mGameCreationDate;
        private Guid _mGameId;
        private int _generalXp;
        
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

        public ICalendarManagement GetProfileCalendar()
        {
            return _calendarModule;
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
                if (jobSupplierObject.StoreUnlockPoints <= GeneralXP)
                {
                    GetActiveJobsModule().UnlockJobModule(jobSupplierObject.BitId);
                }
            }
        }
        private void UpdateItemSuppliersInProfile()
        {
            var itemSuppliersInData = BaseItemSuppliersCatalogue.Instance.GetItemSuppliersInData;
            foreach (var itemSupplier in itemSuppliersInData)
            {
                if (itemSupplier.StoreUnlockPoints <= GeneralXP)
                {
                    GetActiveItemSuppliersModule().UnlockSupplier(itemSupplier.ItemSupplierId);
                }
            }
        }
        private void UpdateItemsInProfile()
        {
            var itemsInCatalogue = ItemsDataController.Instance.ExistingItemsInCatalogue;
            UpdateItemsSpecialStats(itemsInCatalogue);
            var activeProviders = GetActiveItemSuppliersModule().ActiveProviderObjects;
            foreach (var itemSupplier in activeProviders)
            {
                foreach (var suppliersItem in itemsInCatalogue[itemSupplier.Value.BitSupplierId])
                {
                    if (suppliersItem.UnlockPoints <= GeneralXP)
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

        public int GeneralXP
        {
            get => _generalXp;
            set => _generalXp = value;
        }
    }
}