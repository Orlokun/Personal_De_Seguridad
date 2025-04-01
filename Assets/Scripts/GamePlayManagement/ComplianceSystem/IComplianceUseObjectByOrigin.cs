using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.ComplianceSystem
{
    public interface IComplianceUseObjectByOrigin : IComplianceObject
    {
        public List<ItemOrigin> ComplianceConsideredOrigins { get; }
    }
}