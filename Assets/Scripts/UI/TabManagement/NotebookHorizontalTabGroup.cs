using System.Collections.Generic;
using GamePlayManagement.BitDescriptions;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.Interfaces;
using UI.TabManagement.NBVerticalTabs;
using UnityEngine;

namespace UI.TabManagement
{
    public interface INotebookHorizontalTabGroup : ITabGroup
    {
        public List<IVerticaTabElement> JobVerticalTabObjects { get; }
        public List<IVerticaTabElement> SuppliersVerticalTabObjects { get; }
        public List<IVerticaTabElement> LawsTabObjects { get; }
        public List<IVerticaTabElement> SpecialReqVerticalTabObjects { get; }
        public List<IVerticaTabElement> ConfigVerticalTabObjects { get; }

    }

    public class NotebookHorizontalTabGroup : TabGroup, INotebookHorizontalTabGroup
    {
        #region Scriptable List Interface

        private INotebookVerticalTab _verticalTab;
        [SerializeField] private NotebookVerticalTabSource initialSource;

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
        public List<IVerticaTabElement> LawsTabObjects        
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
        
        [SerializeField] private List<GameRequirementTaGroup> specialReqVerticalTabObjects;
        public List<IVerticaTabElement> SpecialReqVerticalTabObjects 
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
        
        [SerializeField] private List<ConfigTabElement> settingsVerticalTabObjects;
        public List<IVerticaTabElement> ConfigVerticalTabObjects 
        {
            get
            {
                var elements = new List<IVerticaTabElement>(); 
                foreach (var jobTabObject in settingsVerticalTabObjects)
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
            _verticalTab = FindFirstObjectByType<NotebookVerticalTabGroup>();
        }

        protected override void Start()
        {
            base.Start();
            _verticalTab.SetNewTabState(initialSource, this, 1);
        }

        public override bool ActivateTabInUI()
        {
            MIsTabActive = true;
            MuiController.ActivateObject(CanvasBitId.Office,(int)OfficePanelsBitStates.NOTEBOOK);
            return MIsTabActive;
        }

        public override bool DeactivateGroupInUI()
        {
            MIsTabActive = false;
            MuiController.DeactivateObject(CanvasBitId.GamePlayCanvas,GameplayPanelsBitStates.ITEM_DETAILED_SIDEBAR);
            return MIsTabActive;
        }

        public override void UpdateItemsContent(int selectedTabIndex, int verticalTabIndex)
        {
            base.UpdateItemsContent(selectedTabIndex, verticalTabIndex);
            _verticalTab.SetNewTabState((NotebookVerticalTabSource)selectedTabIndex, this, verticalTabIndex);
        }
    }
}