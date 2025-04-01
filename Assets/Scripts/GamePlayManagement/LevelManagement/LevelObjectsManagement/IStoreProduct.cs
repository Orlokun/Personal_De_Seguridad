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
}