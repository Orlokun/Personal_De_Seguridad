using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits
{
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSourceData : ScriptableObject
    {
        [SerializeField] private BitGameJobSuppliers jobSupplier;
        [SerializeField] private string _jobNumber;
        [SerializeField] private string _ownerName;
    }
}