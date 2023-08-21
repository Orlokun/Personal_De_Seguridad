using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule
    {
        void AddJobToModule(BitGameJobSuppliers gainedJobSupplier);
    }
}