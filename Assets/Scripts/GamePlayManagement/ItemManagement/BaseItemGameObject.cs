using System;
using ExternalAssets._3DFOV.Scripts;
using GamePlayManagement.BitDescriptions;
using UnityEngine;

namespace GamePlayManagement.ItemManagement
{
    public interface IHasFieldOfView
    {
        public bool HasFieldOfView { get; }
        public IFieldOfView3D FieldOfView3D { get; }
    }

    public abstract class BaseItemGameObject : MonoBehaviour, IBaseItemObject
    {
        public BitItemType ItemType => MItemType;
    
        protected Guid MItemId;
        protected BitItemType MItemType;
    
        protected bool InPlacement;
        public bool IsInPlacement => InPlacement;

        public virtual void SendClickObject()
        {
        }

        #region ItemPlacement
        public virtual void SetInPlacementStatus(bool inPlacement)
        {
            InPlacement = inPlacement;
        }

        public virtual void InitializeItem()
        {
            MItemId = Guid.NewGuid();
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