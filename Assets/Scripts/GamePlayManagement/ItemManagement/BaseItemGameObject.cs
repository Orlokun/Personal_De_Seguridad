using System;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using UnityEngine;

namespace GamePlayManagement.ItemManagement
{
    public abstract class BaseItemGameObject : MonoBehaviour, IBaseItemObject
    {
        public BitItemType ItemType => MItemType;
    
        protected Guid MItemId;
        protected BitItemType MItemType;
    
        protected bool InPlacement;
        public bool IsInPlacement => InPlacement;

        private IItemObject _itemData;
        public IItemObject ItemData => _itemData;

        public virtual void ReceiveFirstClickEvent()
        {
        }

        #region ItemPlacement
        public virtual void SetInPlacementStatus(bool inPlacement)
        {
            InPlacement = inPlacement;
        }

        public virtual void InitializeItem(IItemObject itemData)
        {
            MItemId = Guid.NewGuid();
            _itemData = itemData;
        }
        #endregion
    
        #region SnippetManagement
        private bool hasSnippet = false;
        public bool HasSnippet => hasSnippet;

        public string GetSnippetText { get; }

        public void DisplaySnippet()
        {
        
        }
        #endregion
    }
}