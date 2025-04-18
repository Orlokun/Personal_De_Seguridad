﻿using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.GameRequests
{
    public interface IUseItemByBaseTypeRequest : IGameRequest
    {
        public List<ItemBaseType> RequestConsideredTypes { get; }
    }
}