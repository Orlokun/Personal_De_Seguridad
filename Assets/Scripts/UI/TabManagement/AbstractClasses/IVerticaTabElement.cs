using UnityEngine;

namespace UI.TabManagement.AbstractClasses
{
    public interface IVerticaTabElement
    {
        public string TabElementName { get; }
        public Sprite Icon { get; }
        public void SetName(string tabName);
        void SetIcon(Sprite tabElementIcon);
        void SetSnippetNameText(string tabElementTabElementName);
    }
}