using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits
{
    public interface ISupplierBaseObject
    {
        public string SupplierNumber { get; }
        public string SupplierName { get; }
    }
    public interface IJobSupplierObject : ISupplierBaseObject
    {
        public BitGameJobSuppliers JobSupplier { get; }
        public string OwnerName { get; }
        public Sprite OwnerIcon { get; }
    }
    
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSupplierObject : ScriptableObject, IJobSupplierObject
    {
        [SerializeField] private BitGameJobSuppliers jobSupplier;
        [SerializeField] private string jobNumber;
        [SerializeField] private string jobName;
        [SerializeField] private string ownerName;
        [SerializeField] private Sprite ownerIcon;
        public BitGameJobSuppliers JobSupplier => jobSupplier;
        public string SupplierNumber => jobNumber;
        public string SupplierName => jobName;
        public string OwnerName => ownerName;
        public Sprite OwnerIcon => ownerIcon;
    }
}