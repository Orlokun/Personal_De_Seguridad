using System;
using System.Collections.Generic;
using DataUnits.ItemSources;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.GameCatalogues
{
    public interface IBaseItemSuppliersCatalogue
    {
        public bool SupplierExists(BitItemSupplier itemSupplier);
        public IItemSupplierDataObject GetSupplierFromId(BitItemSupplier itemId);
        public Tuple<bool, int> ItemSupplierPhoneExists(int dialedPhone);
        public IItemSupplierDataObject GetItemSupplierDataFromPhone(int supplierPhone);
        public IItemSupplierDataObject GetItemSupplierData(BitItemSupplier jobSupplier);
        public List<IItemSupplierDataObject> GetItemSuppliersInData { get; }
    }
}