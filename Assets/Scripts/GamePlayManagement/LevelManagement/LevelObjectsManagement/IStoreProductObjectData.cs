using System;
using UnityEngine;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public interface IStoreProduct
    {
        public Vector3 ProductPosition { get; }
        public Transform ProductTransform { get; }
    }
    public interface IStoreProductObjectData
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public int ProductType { get; }
        public int Quantity { get; }
        public int Price { get; }
        public int HideChances { get; }
        public int Tempting { get; }
        public int Punishment { get; }
    }
}