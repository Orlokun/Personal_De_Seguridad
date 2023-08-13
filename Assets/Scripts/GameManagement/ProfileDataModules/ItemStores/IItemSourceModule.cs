using GameManagement.Modules;

namespace GameManagement.ProfileDataModules.ItemStores
{
    public interface IItemSourceModule : IProfileModule
    {
        public int ActiveItemsInSource { get; }
        public bool IsItemActive(int item);
    }
}