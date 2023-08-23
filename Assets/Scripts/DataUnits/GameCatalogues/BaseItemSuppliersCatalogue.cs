using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemSources;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits.GameCatalogues
{
    public class BaseItemSuppliersCatalogue : MonoBehaviour, IBaseItemSuppliersCatalogue
    {
        private static BaseItemSuppliersCatalogue _instance;
        public static IBaseItemSuppliersCatalogue Instance => _instance;
    
        [SerializeField] private List<ItemSupplierDataObject> existingSuppliers;

        private List<IItemSupplierDataObject> _mIItemSuppliers;
        public List<IItemSupplierDataObject> Itemuppliers => _mIItemSuppliers;
    
        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null)
            {
                Destroy(this);
            }
            _instance = this;
            LoadInterfaces();
        }
        private void LoadInterfaces()
        {
            _mIItemSuppliers = new List<IItemSupplierDataObject>();
            foreach (var itemSupplier in existingSuppliers)
            {
                _mIItemSuppliers.Add(itemSupplier);
            }
        }
        
        public bool GetItemSupplerSupplierExists(BitItemSupplier itemSupplier)
        {
            return _mIItemSuppliers.Any(x => x.ItemSupplierId == itemSupplier);
        }

        public IItemSupplierDataObject GetItemSupplierData(BitItemSupplier jobSupplier)
        {
            return _mIItemSuppliers.SingleOrDefault(x => x.ItemSupplierId == jobSupplier);
        }
    }

    public interface IBaseItemSuppliersCatalogue
    {
        public bool GetItemSupplerSupplierExists(BitItemSupplier itemSupplier);
        public IItemSupplierDataObject GetItemSupplierData(BitItemSupplier jobSupplier);

    }
}