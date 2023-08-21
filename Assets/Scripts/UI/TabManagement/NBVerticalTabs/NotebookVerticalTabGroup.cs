using System.Collections.Generic;
using UI.TabManagement.AbstractClasses;
using UnityEngine;

namespace UI.TabManagement.NBVerticalTabs
{
    public class NotebookVerticalTabGroup : TabGroup, INotebookVerticalTab
    {
        private NotebookVerticalTabSource _tabGroupSource;
        [SerializeField]private Transform tabElementsParent;
        [SerializeField] private GameObject tabElementPrefab;
        public void SetNewTabState(NotebookVerticalTabSource newSource, INotebookHorizontalTabGroup parentGroup)
        {
            if (_tabGroupSource == newSource)
            {
                return;
            }
            ClearTabElements();
            
            //Manage Vertical Tabs that should be available when pressing each element in tab
            _tabGroupSource = newSource;
            var verticalTabElements = GetVerticalTabElements(newSource, parentGroup);
            InstantiateVerticalTabs(verticalTabElements);
            UpdateDictionaryData();
        }

        public override bool ActivateTabInUI()
        {
            MIsTabActive = true;
            return MIsTabActive;
        }

        public override bool DeactivateGroupInUI()
        {
            MIsTabActive = false;
            return MIsTabActive;
        }

        #region Private Utils
        private List<IVerticaTabElement> GetVerticalTabElements(NotebookVerticalTabSource newSource, INotebookHorizontalTabGroup parentGroup)
        {
            var verticalTabElements = new List<IVerticaTabElement>();
            _tabGroupSource = newSource;
            switch (_tabGroupSource)
            {
                case NotebookVerticalTabSource.Jobs:
                    verticalTabElements = parentGroup.JobVerticalTabObjects;
                    break;
                case NotebookVerticalTabSource.Suppliers:
                    verticalTabElements = parentGroup.SuppliersVerticalTabObjects;
                    break;
                case NotebookVerticalTabSource.Laws:
                    verticalTabElements = parentGroup.LawsTabObjects;
                    break;
                case NotebookVerticalTabSource.CurrentRequirements:
                    verticalTabElements = parentGroup.SpecialReqVerticalTabObjects;
                    break;
                case NotebookVerticalTabSource.Config:
                    verticalTabElements = parentGroup.ConfigVerticalTabObjects;
                    break;
                default:
                    return verticalTabElements;
            }
            return verticalTabElements;
        }
        private void InstantiateVerticalTabs(List<IVerticaTabElement> verticalTabElements)
        {
            foreach (var tabElement in verticalTabElements)
            {
                var tabGameObject = Instantiate(tabElementPrefab, tabElementsParent);
                var tabElementController = tabGameObject.GetComponent<NotebookVerticalTabElement>();
                tabElementController.SetIcon(tabElement.Icon);
                tabElementController.SetSnippetNameText(tabElement.TabElementName);
                tabElements.Add(tabElementController);
            }
        }
        private void ClearTabElements()
        {
            tabElements.Clear();
            MTabElements.Clear();
            foreach (Transform tabElement in tabElementsParent)
            {
                Destroy(tabElement.gameObject);
            }
        }
        #endregion

        public override void UpdateTabGroupContent(int selectedTabIndex)
        {
            base.UpdateTabGroupContent(selectedTabIndex);
        }
    }
}