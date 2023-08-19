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
            
            List<IVerticaTabElement> verticalTabElements;
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
                    return;
            }

            foreach (var tabElement in verticalTabElements)
            {
                var tabGameObject = Instantiate(tabElementPrefab, tabElementsParent);
                var tabElementController = tabGameObject.GetComponent<NotebookVerticalTabElement>();
                tabElementController.SetIcon(tabElement.Icon);
                tabElementController.SetSnippetNameText(tabElement.TabElementName);
                tabElements.Add(tabElementController);
            }
            UpdateDictionaryData();
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
        
        public override void UpdateTabGroupContent(int selectedTabIndex)
        {
            base.UpdateTabGroupContent(selectedTabIndex);
        }
    }
}