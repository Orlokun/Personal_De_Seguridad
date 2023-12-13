using System;
using UnityEngine;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public interface IStoreProduct
    {
        public void SetStoreProductGameObjectData(IStoreProductObjectData productData, Vector3 productPosition);
        public IStoreProductObjectData GetData { get; }
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
        
        public string PrefabName { get; }
        public string ProductBrand { get; }
        public string ProductSpriteName { get; }
        public string ProductDescription { get; }
    }
}