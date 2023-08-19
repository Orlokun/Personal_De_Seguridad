using UI.TabManagement.TabEnums;
using UnityEngine;

namespace UI.TabManagement.AbstractClasses
{
    [CreateAssetMenu(menuName = "TabElements/ConfigTabElement")]
    public class ConfigTabElement : ScriptableObject, IVerticaTabElement
    {
        [SerializeField] private ConfigSourcesTabs tabType;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string tabElementName;
        public Sprite Icon => icon;
        public string TabElementName => tabElementName;
    }
}