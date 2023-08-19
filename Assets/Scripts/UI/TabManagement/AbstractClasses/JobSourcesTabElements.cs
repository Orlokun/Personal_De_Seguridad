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
    }

    public interface IVerticaTabElement
    {
        public Sprite Icon { get; }
        public String TabElementName { get; }
    }
}