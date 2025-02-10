using System.Collections.Generic;
using GamePlayManagement.BitDescriptions;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.Interfaces;
using UI.TabManagement.TabEnums;
using UnityEngine;

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

    public class NotebookHorizontalTabGroup : TabGroup, INotebookHorizontalTabletTabGroup
    {
        #region Scriptable List Interface

        private INotebookVerticalTabGroup _mVerticalTabsGroupGroup;
        [SerializeField] private NotebookHorizontalTabSource initialSource;

        public List<JobSourcesTabElements> jobVerticalTabObjects;
        public List<IVerticaTabElement> JobVerticalTabObjects
        {
            get
            {
                var elements = new List<IVerticaTabElement>(); 
                foreach (var jobTabObject in jobVerticalTabObjects)
                {
                    elements.Add(jobTabObject);
                }
                return elements;
            }
        }

        [SerializeField] private List<SuppliersTabElement> suppliersVerticalTabObjects;
        public List<IVerticaTabElement> SuppliersVerticalTabObjects
        {
            get
            {
                var elements = new List<IVerticaTabElement>(); 
                foreach (var jobTabObject in suppliersVerticalTabObjects)
                {
                    elements.Add(jobTabObject);
                }
                return elements;
            }
        }
        
        [SerializeField] private List<LawsTabElement> lawsVerticalTabObjects;
        public List<IVerticaTabElement> ComplianceTabObjects        
        {
            get
            {
                var elements = new List<IVerticaTabElement>(); 
                foreach (var jobTabObject in lawsVerticalTabObjects)
                {
                    elements.Add(jobTabObject);
                }
                return elements;
            }
        }
        
        [SerializeField] private List<GameRequirementsTabElement> specialReqVerticalTabObjects;
        public List<IVerticaTabElement> RequirementsTabObjects 
        {
            get
            {
                var elements = new List<IVerticaTabElement>(); 
                foreach (var jobTabObject in specialReqVerticalTabObjects)
                {
                    elements.Add(jobTabObject);
                }
                return elements;
            }
        }
        
        [SerializeField] private List<OmniScrollerTabElement> omniScrollerTabObjects;
        public List<IVerticaTabElement> ConfigVerticalTabObjects 
        {
            get
            {
                var elements = new List<IVerticaTabElement>(); 
                foreach (var jobTabObject in omniScrollerTabObjects)
                {
                    elements.Add(jobTabObject);
                }
                return elements;
            }
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();
            _mVerticalTabsGroupGroup = FindFirstObjectByType<NotebookVerticalTabGroup>();
        }

        protected override void Start()
        {
            base.Start();
            _mVerticalTabsGroupGroup.SetNewTabState(initialSource, this, 1);
        }

        public override void ActivateTabletUI()
        {
            MuiController.ActivateObject(CanvasBitId.Office,(int)OfficePanelsBitStates.NOTEBOOK);
        }

        public override bool DeactivateItemsDetailBar()
        {
            MIsTabActive = false;
            MuiController.DeactivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
            return MIsTabActive;
        }

        public override void UpdateItemsContent(int horizontalTabIndex, int verticalTabIndex)
        {
            MActiveTab = horizontalTabIndex;
            _mVerticalTabsGroupGroup.SetNewTabState((NotebookHorizontalTabSource)horizontalTabIndex, this, verticalTabIndex);
        }
    }
}