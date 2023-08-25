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
        public IItemSuppliersModule GetActiveSuppliersModule();
        public IJobsSourcesModule GetActiveJobModule();
    }
}