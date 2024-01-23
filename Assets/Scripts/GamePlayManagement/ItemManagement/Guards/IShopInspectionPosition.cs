using System;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IShopInspectionPosition : IInitialize
    {
        public Vector3 Position { get; }
        public Guid Id { get; }
        public IShopInspectionPosition NextInspectionPosition { get; }
        public IShopInspectionPosition PreviousInspectionPosition { get; }
    }
}