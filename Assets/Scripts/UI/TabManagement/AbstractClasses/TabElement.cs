using System;
using JetBrains.Annotations;
using TMPro;
using UI.TabManagement.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
using Utils;

namespace UI.TabManagement.AbstractClasses
{
    public class TabElement : MonoBehaviour, ITabElement, IInitializeWithArg1<ITabGroup>
    {
        protected bool MInitialized;
        protected ITabGroup TabGroup;
        protected int MTabElementIndex;

        [CanBeNull] protected string MSnippetName;
        [SerializeField] private TMP_Text snippetNameText;
        private Image _mBackground;

        public void SetSnippetNameText(string snippetName)
        {
            MSnippetName = snippetName;
            snippetNameText.text = MSnippetName;
        }
        
        public virtual void TabSelected()
        {
            throw new NotImplementedException("Tab Selected Must be used by inheritors.");
        }

        public bool IsInitialized => MInitialized;
        public virtual void Initialize(ITabGroup injectionClass)
        {
            if (MInitialized)
            {
                return;
            }
            TabGroup = injectionClass;
            MInitialized = true;
        }

        public void SetTabIndex(int index)
        {
            MTabElementIndex = index;
        }
    }
}