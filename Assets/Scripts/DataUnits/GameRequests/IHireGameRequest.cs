using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.GameRequests
{
    public interface IHireGameRequest : IGameRequest
    {
        public JobSupplierBitId JobHireObjective { get; }
    }
}