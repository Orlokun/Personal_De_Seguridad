using UnityEngine;
namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class StoreProductGameObject : MonoBehaviour, IStoreProduct
    {
        protected Vector3 MProductPosition;
        public Vector3 ProductPosition => transform.position;
        public Transform ProductTransform => transform;

        public IStoreProductObjectData GetData => _data;
        private IStoreProductObjectData _data;
        
        public void SetStoreProductGameObjectData(IStoreProductObjectData productData, Vector3 productPosition)
        {
            _data = productData;
            MProductPosition = productPosition;
        }
    }
}