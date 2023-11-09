using GamePlayManagement.BitDescriptions.Suppliers;
using UI;
using UnityEngine;

namespace DataUnits.ItemSources
{
    [CreateAssetMenu(menuName = "ItemSource/BaseItemSource")]
    public class ItemSupplierDataObject : ScriptableObject, IItemSupplierDataObject
    {
        
        [SerializeField] protected Sprite supplierPortrait;
        public Sprite SupplierPortrait =>supplierPortrait;

        
        public int StorePhoneNumber { get; set; }
        public string StoreName { get; set; }
        public string StoreDescription { get; set; }
        public BitItemSupplier ItemSupplierId { get; set; }
        public int ItemTypesAvailable { get; set; }
        public int UnlockPoints { get; set; }
    }

    public interface ICallableObject
    {
    }

    public interface IItemSupplierDataObject : ISupplierBaseObject
    {
        public BitItemSupplier ItemSupplierId { get; set; }
        public int UnlockPoints { get; set; }
        public int ItemTypesAvailable { get; set; }

    }
}