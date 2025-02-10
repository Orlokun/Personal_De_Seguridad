using UI.TabManagement.TabEnums;
using UnityEngine;

namespace UI.TabManagement.AbstractClasses
{
    [CreateAssetMenu(menuName = "TabElements/ConfigTabElement")]
    public class OmniScrollerTabElement : ScriptableObject, IVerticaTabElement
    {
        [SerializeField] private ConfigSourcesTabs tabType;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string tabElementName;
        public Sprite Icon => icon;
        public string TabElementName => tabElementName;
        public void SetName(string tabName)
        {
            throw new System.NotImplementedException();
        }

        public void SetIcon(Sprite tabElementIcon)
        {
            throw new System.NotImplementedException();
        }

        public void SetSnippetNameText(string tabElementTabElementName)
        {
            throw new System.NotImplementedException();
        }
    }
}