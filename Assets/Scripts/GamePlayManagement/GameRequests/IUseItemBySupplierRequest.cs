using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.GameRequests
{
    public interface IUseItemBySupplierRequest
    {
        public BitItemSupplier RequestConsideredSupplier  { get; }
    }    
    
    public interface IUseExactItemRequest
    {
        public BitItemSupplier RequestConsideredSupplier  { get; }
        public int RequestConsideredItemId { get; }
    }    
    
    public interface IUseItemWithVariableRequest
    {
        public BitItemSupplier RequestConsideredSupplier  { get; }
        public int RequestConsideredItemId { get; }
    }
}