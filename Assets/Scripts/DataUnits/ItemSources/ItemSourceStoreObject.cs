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
    
        public BitItemSupplier ItemSupplier =>itemSupplier;
        public string SupplierName =>supplierName;
        public string SupplierPhone =>supplierPhone;
        public Sprite SupplierPortrait =>supplierPortrait;
    }

    public interface IItemSupplierDataObject
    {
        public BitItemSupplier ItemSupplier { get; }
        public string SupplierName{ get; }
        public string SupplierPhone{ get; }
        public Sprite SupplierPortrait{ get; }
    }
}