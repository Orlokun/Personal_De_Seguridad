﻿using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.ComplianceSystem
{
    public interface IComplianceUseObjectByBaseType : IComplianceObject
    {
        public List<ItemBaseType> ComplianceConsideredBaseTypes { get; }
    }
    public interface IComplianceUseObjectByQuality : IComplianceObject
    {
        public ItemBaseQuality ItemQuality { get; }
    }
}