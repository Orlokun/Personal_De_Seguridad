using System.Collections.Generic;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.Interfaces;

namespace UI.TabManagement.NotebookTabs.HorizontalTabletTabs
{
    public interface INotebookHorizontalTabletTabGroup : ITabletTabGroup
    {
        public List<IVerticaTabElement> JobVerticalTabObjects { get; }
        public List<IVerticaTabElement> SuppliersVerticalTabObjects { get; }
        public List<IVerticaTabElement> ComplianceTabObjects { get; }
        public List<IVerticaTabElement> RequirementsTabObjects { get; }
        public List<IVerticaTabElement> ConfigVerticalTabObjects { get; }
    }
}