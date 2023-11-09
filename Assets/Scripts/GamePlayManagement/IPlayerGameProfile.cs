using System;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using UI;

namespace GamePlayManagement
{
    public interface IPlayerGameProfile
    {
        public DateTime GameCreationDate { get; }
        public Guid GameId { get; }
        public IItemSuppliersModule GetActiveItemSuppliersModule();
        public IJobsSourcesModule GetActiveJobModule();
        public void UpdateProfileData();
        public int GeneralXP { get; set; }
    }
    
    
}