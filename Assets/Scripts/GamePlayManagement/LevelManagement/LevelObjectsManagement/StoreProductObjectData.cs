using System;
using UnityEngine;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class StoreProductGameObject : MonoBehaviour, IStoreProduct
    {
        protected Vector3 MProductPosition;
        protected Transform MProductTransform; 
        public Vector3 ProductPosition => transform.position;
        public Transform ProductTransform => transform;

        public StoreProductObjectData GetData => _data;
        private StoreProductObjectData _data;
        
        public StoreProductGameObject(StoreProductObjectData productData)
        {
            _data = productData;
        }
    }
    
    public class StoreProductObjectData :  IStoreProductObjectData
    {
        protected int MProductId;
        protected string MProductName;
        protected int MProductType;
        protected int MQuantity;

        protected int MPrice;
        protected int MHideChances;
        protected int MTempting;
        protected int MPunishment;

        public StoreProductObjectData(int id, string name, int type, int quantity, int price, int hideChances, int tempting, int punishment,
                                        string prefabName, string productBrand, string productSpriteName,string productDescription)
        {
            MProductId = id;
            MProductName = name;

            MProductType = type;
            MQuantity = quantity;
            MPrice = price;
            MHideChances = hideChances;
            MTempting = tempting;
            MPunishment = punishment;
        }
        
        public int ProductId => MProductId;
        public string ProductName => MProductName;
        public int ProductType => MProductType;
        public int Quantity => MQuantity;

        public int Price => MPrice;
        public int HideChances => MHideChances;
        public int Tempting => MTempting;
        public int Punishment => MPunishment;
    }
}