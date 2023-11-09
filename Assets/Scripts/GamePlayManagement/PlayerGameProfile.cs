using System;
using DataUnits.GameCatalogues;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using UnityEngine;

namespace GamePlayManagement
{
    public class PlayerGameProfile : IPlayerGameProfile
    {
        //Profile constructor
        public PlayerGameProfile(IItemSuppliersModule itemSuppliersModule, IJobsSourcesModule jobsSourcesModule)
        {
            _itemSuppliersModule = itemSuppliersModule;
            _jobsSourcesModule = jobsSourcesModule;
            _mGameCreationDate = DateTime.Now;
            _mGameId = Guid.NewGuid();
            _generalXp = 3;
        }
        
        //Main Data Modules
        private IItemSuppliersModule _itemSuppliersModule;
        private IJobsSourcesModule _jobsSourcesModule;

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
        public IJobsSourcesModule GetActiveJobModule()
        {
            return _jobsSourcesModule;
        }
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
                    GetActiveJobModule().UnlockJobModule(jobSupplierObject.BitId);
                }
            }
        }
        private void UpdateItemSuppliersInProfile()
        {
            var itemSuppliersInData = BaseItemSuppliersCatalogue.Instance.GetItemSuppliersCompleteData;
            foreach (var itemSupplier in itemSuppliersInData)
            {
                if (itemSupplier.UnlockPoints <= GeneralXP)
                {
                    GetActiveItemSuppliersModule().UnlockSupplier(itemSupplier.ItemSupplierId);
                }
            }
        }
        private void UpdateItemsInProfile()
        {
            var itemsInCatalogue = BaseItemCatalogue.Instance.ExistingItemsInCatalogue;
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
        public int GeneralXP
        {
            get => _generalXp;
            set => _generalXp = value;
        }
    }
}