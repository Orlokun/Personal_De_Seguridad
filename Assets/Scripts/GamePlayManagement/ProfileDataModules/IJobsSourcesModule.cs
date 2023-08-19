using GameManagement.BitDescriptions.Suppliers;
using GameManagement.Modules;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.ProfileDataModules
{
    public interface IJobsSourcesModule : IProfileModule
    {
        void AddJobToModule(GameJobs gainedJob);
    }
}