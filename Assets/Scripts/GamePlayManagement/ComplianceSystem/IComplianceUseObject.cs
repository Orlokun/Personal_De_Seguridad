using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.ComplianceSystem
{
    public interface IComplianceUseObject : IComplianceObject
    {
        public List<ItemOrigin> ComplianceConsideredOrigins { get; }
    }
}