using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits.ItemSources
{
    [CreateAssetMenu(menuName = "ItemSource/BaseItemSource")]
    public class ItemSupplierDataObject : ScriptableObject, IItemSupplierDataObject
    {
        [SerializeField] protected BitItemSupplier itemSupplier;
        [SerializeField] protected string supplierName;
        [SerializeField] protected string supplierPhone;
        [SerializeField] protected Sprite supplierPortrait;
    
        public BitItemSupplier ItemSupplierId =>itemSupplier;
        public string SupplierName =>supplierName;
        public string SupplierNumber => supplierPhone;
        public Sprite SupplierPortrait =>supplierPortrait;
    }

    public interface ICallableObject
    {
    }

    public interface IItemSupplierDataObject : ISupplierBaseObject
    {
        public BitItemSupplier ItemSupplierId { get; }
        public Sprite SupplierPortrait{ get; }
    }
}