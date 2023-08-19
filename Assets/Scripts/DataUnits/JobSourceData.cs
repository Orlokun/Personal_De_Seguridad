using GameManagement.BitDescriptions.Suppliers;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits
{
    [CreateAssetMenu(menuName = "Jobs/JobSource")]
    public class JobSourceData : ScriptableObject
    {
        [SerializeField] private GameJobs _job;
        [SerializeField] private string _jobNumber;
        [SerializeField] private string _ownerName;
    }
}