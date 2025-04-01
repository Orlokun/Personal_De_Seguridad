using System.Threading.Tasks;
using DataUnits.JobSources;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules.ItemSuppliers.Stores;

namespace DataUnits.ItemSources
{
    public interface IItemSupplierDataObject : ISupplierBaseObject, ICallableSupplier
    {
        public BitItemSupplier ItemSupplierId { get; set; }
        public int StoreUnlockPoints { get; set; }
        public int ItemTypesAvailable { get; set; }
        public string StoreDescription { get; set; }
        public void LoadDeflectionsDialogueData();
        public void SetStats(int reliance, int quality, int kindness, int stockRefillPeriod, string spriteName);
        public int Reliance { get; }
        public int Quality { get; }
        public int Kindness { get; }
        public int RefillStockPeriod { get; }
        public Task StartUnlockedData();
        public void InitializeStore(IItemSupplierShop shop);
        public IItemSupplierShop SupplierShop { get; }
    }
}