using UI.TabManagement.TabEnums;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.TabManagement.AbstractClasses
{
    [CreateAssetMenu(menuName = "TabElements/SpecialReqTabElement")]
    public class GameRequirementTaGroup : ScriptableObject, IVerticaTabElement
    {
        [SerializeField] private RequestTabSources type;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string tabElementName;
        public Sprite Icon => icon;
        public string TabElementName => tabElementName;
    }
}