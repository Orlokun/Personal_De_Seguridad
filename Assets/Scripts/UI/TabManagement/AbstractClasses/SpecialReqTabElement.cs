using UI.TabManagement.TabEnums;
using UnityEngine;

namespace UI.TabManagement.AbstractClasses
{
    [CreateAssetMenu(menuName = "TabElements/SpecialReqTabElement")]
    public class SpecialReqTabElement : ScriptableObject, IVerticaTabElement
    {
        [SerializeField] private SpecialReqSourcesTabs tabType;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string tabElementName;
        public Sprite Icon => icon;
        public string TabElementName => tabElementName;
    }
}