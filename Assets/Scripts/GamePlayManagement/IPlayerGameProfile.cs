using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using UI;

namespace GamePlayManagement
{
    public interface IPlayerGameProfile
    {
        public IItemSuppliersModule GetActiveSuppliersModule();
        public IJobsSourcesModule GetActiveJobModule();
    }
}