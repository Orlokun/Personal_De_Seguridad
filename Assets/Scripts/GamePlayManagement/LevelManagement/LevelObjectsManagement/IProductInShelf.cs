using System;
using UnityEngine;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public interface IProductInShelf
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public Vector3 ProductPosition { get; }
        public Transform ProductTransform { get; }


        public int Price { get; }
        public int Temptation { get; }
        public int Visibility { get; }
    }
}