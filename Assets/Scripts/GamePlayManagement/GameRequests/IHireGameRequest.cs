using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.GameRequests
{
    public interface IHireGameRequest : IGameRequest
    {
        public JobSupplierBitId JobHireObjective { get; }
    }
}