using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.GameRequests
{
    public interface IUseSpecificItemRequest : IGameRequest
    {
        public BitItemSupplier ItemOwner { get; }
        public int ItemId { get; }
    }
}