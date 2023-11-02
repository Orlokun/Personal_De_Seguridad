using UnityEngine;

namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class ProductPositionInShelf
    {
        public ProductPositionInShelf(Transform productPosition)
        {
            _mpositionInShelf = productPosition.position;
        }
    
        private Vector3 _mpositionInShelf;
        public Vector3 PositionInShelf => _mpositionInShelf;

        private bool isOccupied;
        public bool IsOccupied => isOccupied;
        
        public void ToggleOccupy(bool newOccupation)
        {
            isOccupied = newOccupation;
        }
    }
}