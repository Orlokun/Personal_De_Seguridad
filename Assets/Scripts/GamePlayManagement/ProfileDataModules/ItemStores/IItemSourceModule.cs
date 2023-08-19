using GameManagement.Modules;

namespace GamePlayManagement.ProfileDataModules.ItemStores
{
    public interface IItemSourceModule : IProfileModule
    {
        public int ActiveItemsInSource { get; }
        public bool IsItemActive(int item);
        
    }
}