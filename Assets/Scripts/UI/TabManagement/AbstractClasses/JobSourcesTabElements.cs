using System;
using UI.TabManagement.TabEnums;
using UnityEngine;

namespace UI.TabManagement.AbstractClasses
{
    [CreateAssetMenu(menuName = "TabElements/JobSourcesTabElements")]
    public class JobSourcesTabElements : ScriptableObject, IVerticaTabElement
    {
        [SerializeField] private JobSourcesTabs tabType;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string tabElementName;

        public Sprite Icon => icon;
        public string TabElementName => tabElementName;
        public void SetName(string tabName)
        {
            throw new NotImplementedException();
        }

        public void SetIcon(Sprite tabIcon)
        {
            
        }

        public void SetSnippetNameText(string tabElementTabElementName)
        {
            
        }
    }

    public interface IVerticaTabElement
    {
        public string TabElementName { get; }
        public Sprite Icon { get; }
        public void SetName(string tabName);
        void SetIcon(Sprite tabElementIcon);
        void SetSnippetNameText(string tabElementTabElementName);
    }
}