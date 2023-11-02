using System;
using UnityEngine;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class ProductInShelf : MonoBehaviour, IProductInShelf
    {
        public Guid ProductId { get; }
        public string ProductName { get; }
        public Vector3 ProductPosition { get; }
        public Transform ProductTransform => transform;

        public int Price { get; }
        public int Temptation { get; }
        public int Visibility { get; }
    }
}