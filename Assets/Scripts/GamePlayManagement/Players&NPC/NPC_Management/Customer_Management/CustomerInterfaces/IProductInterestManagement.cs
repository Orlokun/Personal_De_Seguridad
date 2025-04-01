using System;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using UnityEngine;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface IProductInterestManagement
    {
        public Tuple<Transform, IStoreProductObjectData> TempStoreProductOfInterest { get; }
    }
}