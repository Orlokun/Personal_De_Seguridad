
using UI.TabManagement.TabEnums;
using UnityEngine;

namespace UI.TabManagement.AbstractClasses
{
    [CreateAssetMenu(menuName = "TabElements/SuppliersTabElement")]
    public class SuppliersTabElement : ScriptableObject, IVerticaTabElement
    {
        [SerializeField] private SupplierSourcesTabs tabType;
        [SerializeField] protected Sprite icon;
        [SerializeField] protected string tabElementName;
        public Sprite Icon => icon;
        public string TabElementName => tabElementName;
    }
}