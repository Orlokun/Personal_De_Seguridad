using System;
using UnityEngine;

namespace GamePlayManagement.ItemManagement.Guards
{
    public class ShopInspectionPosition : MonoBehaviour, IShopInspectionPosition
    {
        [SerializeField] private ShopInspectionPosition mNextPosition;
        [SerializeField] private ShopInspectionPosition mPreviousPosition;

        public IShopInspectionPosition NextInspectionPosition => mNextPosition;
        public IShopInspectionPosition PreviousInspectionPosition => mPreviousPosition;
        
        public Vector3 Position => transform.position;
        public Guid Id => _mId;

        private Guid _mId;

        private bool _mIsInitialized;
        public bool IsInitialized => _mIsInitialized;

        public void Initialize()
        {
            _mId = Guid.NewGuid();
            _mIsInitialized = true;
        }
    }
}