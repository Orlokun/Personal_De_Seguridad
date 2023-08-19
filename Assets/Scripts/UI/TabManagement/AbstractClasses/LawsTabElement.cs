using UI.TabManagement.TabEnums;
using UnityEngine;

namespace UI.TabManagement.AbstractClasses
{
    [CreateAssetMenu(menuName = "TabElements/LawsTabElement")]
    public class LawsTabElement : ScriptableObject, IVerticaTabElement
    {
        [SerializeField] private LawsSourcesTabs tabType;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string tabElementName;
        public Sprite Icon => icon;
        public string TabElementName => tabElementName;
    }
}