using System;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using Utils;

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
        }
        
        //Main Data Modules
        private IItemSuppliersModule _itemSuppliersModule;
        private IJobsSourcesModule _jobsSourcesModule;

        //Members
        private DateTime _mGameCreationDate;
        private Guid _mGameId;
        
        //Public Fields
        public DateTime GameCreationDate => _mGameCreationDate;
        public Guid GameId => _mGameId;
        
        //Public Module Interfaces Fields
        public IItemSuppliersModule GetActiveSuppliersModule()
        {
            return _itemSuppliersModule;
        }

        public IJobsSourcesModule GetActiveJobModule()
        {
            return _jobsSourcesModule;
        }
    }
}