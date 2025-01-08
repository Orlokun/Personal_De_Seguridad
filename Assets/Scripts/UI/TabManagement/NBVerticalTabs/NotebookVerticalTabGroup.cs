using System.Collections.Generic;
using GameDirection;
using GamePlayManagement;
using UI.TabManagement.AbstractClasses;
using UnityEngine;

namespace UI.TabManagement.NBVerticalTabs
{
    public class NotebookVerticalTabGroup : TabGroup, INotebookVerticalTab
    {
        private NotebookVerticalTabSource _tabGroupSource;
        private IPlayerGameProfile _playerProfile;
        [SerializeField]private Transform tabElementsParent;
        [SerializeField] private GameObject tabElementPrefab;

        [SerializeField] private Transform leftPage;
        [SerializeField] private Transform rightPage;

        [SerializeField] private GameObject JobPrefab;
        [SerializeField] private GameObject SupplierPrefab;
        [SerializeField] private GameObject LawsPrefab;
        [SerializeField] private GameObject RequirementsPrefab;
        [SerializeField] private GameObject NewsPrefab;
        
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
            UpdateItemsContent((int)newSource);
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
                case NotebookVerticalTabSource.News:
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

        private void ClearNotebookContent()
        {
            foreach (Transform leftPageContent in leftPage)
            {
                Destroy(leftPageContent.gameObject);
            }
            foreach (Transform leftPageContent in rightPage)
            {
                Destroy(leftPageContent.gameObject);
            }
        }

        private GameObject InstantiatePrefabs(int i, GameObject prefab)
        {
            return i < 5 ? Instantiate(prefab, leftPage) : Instantiate(JobPrefab, rightPage);
        }
        
        private void UpdateContentInPage(NotebookVerticalTabSource notebookInfoSource)
        {
            switch (notebookInfoSource)
            {
                case NotebookVerticalTabSource.Jobs:
                    ManageJobObjectsInstantiation();
                    break;
                case NotebookVerticalTabSource.Suppliers:
                    ManageSuppliersInstantiation();
                    break;
                case NotebookVerticalTabSource.Laws:
                    break;
                case NotebookVerticalTabSource.CurrentRequirements:
                    break;
                case NotebookVerticalTabSource.News:
                    ManageNewsInstantiation();
                    break;
                default:
                    return;
            }
        }
        private void ManageJobObjectsInstantiation()
        {
            var availableJobSources = _playerProfile.GetActiveJobsModule().JobObjects;
            var index = 0;
            foreach (var availableJobSource in availableJobSources)
            {
                if (index ==10)
                {
                    break;
                }
                var prefabObject = InstantiatePrefabs(index, JobPrefab);
                var jobObjectController = prefabObject.GetComponent<NotebookSupplierObject>();
                        
                jobObjectController.SetNotebookObjectValues(availableJobSource.Value);
                index++;
            }
        }

        private void ManageSuppliersInstantiation()
        {
            var availableItemSuppliers = _playerProfile.GetActiveItemSuppliersModule().ActiveProviderObjects;
            var index = 0;
            foreach (var availableItemSupplier in availableItemSuppliers)
            {
                if (index ==10)
                {
                    break;
                }
                var prefabObject = InstantiatePrefabs(index, SupplierPrefab);
                var supplierObjectController = prefabObject.GetComponent<NotebookSupplierObject>();
                //var storeIcon = availableItemSupplier.Value.GetSupplierData.SupplierPortrait;
                supplierObjectController.SetNotebookObjectValues(availableItemSupplier.Value.GetSupplierData);
                index++;
            }
        }
        private void ManageNewsInstantiation()
        {
            var currentDay = _playerProfile.GetProfileCalendar().GetCurrentWorkDayObject().BitId;
            var dayNews = GameDirector.Instance.GetNarrativeNewsDirector.GetDayNews(currentDay);
            var index = 0;
            foreach (var news in dayNews)
            {
                if (index ==10)
                {
                    break;
                }
                var prefabObject = InstantiatePrefabs(index, NewsPrefab);
                var newsComponent = prefabObject.GetComponent<INewsObjectPrefab>();
                newsComponent.PopulateNewsPrefab(news);
                index++;
            }
        }
        #endregion

        public override void UpdateItemsContent(int selectedTabIndex)
        {
            if (_playerProfile == null)
            {
                _playerProfile = GameDirector.Instance.GetActiveGameProfile;
            }
            base.UpdateItemsContent(selectedTabIndex);
            ClearNotebookContent();
            UpdateContentInPage(_tabGroupSource);
        }
    }
}