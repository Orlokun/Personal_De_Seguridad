using System.Collections.Generic;
using DialogueSystem.Units;
using GameDirection;
using GamePlayManagement;
using GamePlayManagement.ComplianceSystem;
using GamePlayManagement.GameRequests;
using UI.TabManagement.AbstractClasses;
using UI.TabManagement.NotebookTabs.CompliancePrefab;
using UI.TabManagement.NotebookTabs.HorizontalTabletTabs;
using UI.TabManagement.TabEnums;
using UnityEngine;

namespace UI.TabManagement.NotebookTabs
{
    public class NotebookVerticalTabGroup : TabGroup, INotebookVerticalTabGroup
    {
        private NotebookHorizontalTabSource _mCurrentHorizontalSource;
        private INotebookHorizontalTabletTabGroup _mHorizontalTabletTabGroup;
        
        private IPlayerGameProfile _playerProfile;
        [SerializeField]private Transform tabElementsParent;
        [SerializeField] private GameObject tabElementPrefab;

        [SerializeField] private Transform leftPage;
        [SerializeField] private Transform rightPage;

        [SerializeField] private GameObject JobPrefab;
        [SerializeField] private GameObject SupplierPrefab;
        [SerializeField] private GameObject CompliancePrefab;
        [SerializeField] private GameObject RequirementsPrefab;
        [SerializeField] private GameObject NewsPrefab;

        private List<IVerticaTabElement> _mTabElements = new List<IVerticaTabElement>();
        
        protected override void Awake()
        {
            base.Awake();
            _mHorizontalTabletTabGroup = FindFirstObjectByType<NotebookHorizontalTabGroup>(FindObjectsInactive.Include);
        }
        public void SetNewTabState(NotebookHorizontalTabSource newSource, INotebookHorizontalTabletTabGroup parentGroup, int verticalTabIndex)
        {
            Debug.Log($"Setting new tab state: {newSource}");
            if (_mCurrentHorizontalSource == newSource)
            {
                return;
            }
            _mHorizontalTabletTabGroup ??= parentGroup;
            ClearVerticalTabsData();

            //Manage Vertical Tabs that should be available when pressing each element in tab
            _mCurrentHorizontalSource = newSource;
            _mTabElements = GetVerticalTabElements(parentGroup);
            tabElements.Clear();
            InstantiateVerticalTabs();
            UpdateItemsContent(verticalTabIndex);
        }

        public void UpdateTabSelection(int verticalTabIndex)
        {
            MActiveTab = verticalTabIndex;
            UpdateItemsContent(verticalTabIndex);
        }


        #region Private Utils

        #region TabManagement
        private List<IVerticaTabElement> GetVerticalTabElements(INotebookHorizontalTabletTabGroup parentGroup)
        {
            var verticalTabElements = new List<IVerticaTabElement>();
            switch (_mCurrentHorizontalSource)
            {
                case NotebookHorizontalTabSource.Jobs:
                    verticalTabElements = parentGroup.JobVerticalTabObjects;
                    break;
                case NotebookHorizontalTabSource.Suppliers:
                    verticalTabElements = parentGroup.SuppliersVerticalTabObjects;
                    break;
                case NotebookHorizontalTabSource.Compliance:
                    verticalTabElements = parentGroup.ComplianceTabObjects;
                    break;
                case NotebookHorizontalTabSource.CurrentRequirements:
                    verticalTabElements = parentGroup.RequirementsTabObjects;
                    break;
                case NotebookHorizontalTabSource.OmniScroll:
                    verticalTabElements = parentGroup.ConfigVerticalTabObjects;
                    break;
                default:
                    return verticalTabElements;
            }
            return verticalTabElements;
        }
        private void InstantiateVerticalTabs()
        {
            var verticalIndex = 1;
            foreach (var tabElement in _mTabElements)
            {
                var tabGameObject = Instantiate(tabElementPrefab, tabElementsParent);
                var tabElementController = tabGameObject.GetComponent<NotebookVerticalTabElement>();
                tabElementController.SetIcon(tabElement.Icon);
                tabElementController.SetName(tabElement.TabElementName);
                tabElementController.SetSnippetNameText(tabElement.TabElementName);
                tabElements.Add((TabElement)tabElementController);
            }
            UpdateDictionaryData();
        }
        private void ClearVerticalTabsData()
        {
            _mTabElements.Clear();
            foreach (Transform tabElement in tabElementsParent)
            {
                Destroy(tabElement.gameObject);
            }
        }
        #endregion

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
            return i < 5 ? Instantiate(prefab, leftPage) : Instantiate(prefab, rightPage);
        }
        
        private void UpdateContentInPage(NotebookHorizontalTabSource notebookInfoSource, int selectedTab)
        {
            switch (notebookInfoSource)
            {
                case NotebookHorizontalTabSource.Jobs:
                    ManageJobObjectsInstantiation(selectedTab);
                    break;
                case NotebookHorizontalTabSource.Suppliers:
                    ManageSuppliersInstantiation(selectedTab);
                    break;
                case NotebookHorizontalTabSource.Compliance:
                    var complianceTabSource = (RequestTabSources)selectedTab;
                    ManageComplianceInstantiation(complianceTabSource);
                    break;
                case NotebookHorizontalTabSource.CurrentRequirements:
                    var requirementsTabSource = (RequestTabSources)selectedTab;
                    ManageRequestObjectsInstantiation(requirementsTabSource);
                    break;
                case NotebookHorizontalTabSource.OmniScroll:
                    ManageNewsInstantiation();
                    break;
                default:
                    return;
            }
        }

        private void ManageComplianceInstantiation(RequestTabSources selectedTab)
        {
            var complianceManager = GameDirector.Instance.GetActiveGameProfile.GetComplianceManager;
            var currentDay = _playerProfile.GetProfileCalendar().CurrentDayBitId;
            List<IComplianceObject> complianceObjectsToInstantiate;

            switch (selectedTab)
            {
                case RequestTabSources.ActiveRequests:
                    complianceObjectsToInstantiate = complianceManager.GetActiveComplianceObjects;
                    break;
                case RequestTabSources.CompletedRequests:
                    complianceObjectsToInstantiate = complianceManager.GetCompletedComplianceObjects;
                    break;
                case RequestTabSources.FailedRequests:
                    complianceObjectsToInstantiate = complianceManager.GetFailedComplianceObjects;
                    break;
                default:
                    return;
            }
            
            for (var i = 0; i < complianceObjectsToInstantiate.Count; i++)
            {
                var complianceObject = complianceObjectsToInstantiate[i];
                var prefabObject = InstantiatePrefabs(i, CompliancePrefab);
                var requirementObjectController = prefabObject.GetComponent<IComplianceObjectPrefab>();
                requirementObjectController.PopulateCompliancePrefab(complianceObject);
            }
        }

        private void ManageRequestObjectsInstantiation(RequestTabSources selectedTab)
        {
            var reqManager = _playerProfile.GetRequestsModuleManager();
            Dictionary<DialogueSpeakerId, List<IGameRequest>> activeRequesters;

            switch (selectedTab)
            {
                case RequestTabSources.ActiveRequests:
                    activeRequesters = reqManager.ActiveRequests;
                    break;
                case RequestTabSources.CompletedRequests:
                    activeRequesters = reqManager.CompletedRequests;
                    break;
                case RequestTabSources.FailedRequests:
                    activeRequesters = reqManager.FailedRequests;
                    break;
                default:
                    return;
            }
            
            var index = 0;
            foreach (var activeRequester in activeRequesters)
            {
                foreach (var activeRequest in activeRequester.Value)
                {
                    if (index ==10)
                    {
                        break;
                    }
                    var prefabObject = InstantiatePrefabs(index, RequirementsPrefab);
                    var requirementObjectController = prefabObject.GetComponent<IRequirementObjectPrefab>();
                    requirementObjectController.PopulateRequestPrefab(activeRequest);
                    index++;
                }
            }
        }

        private void ManageJobObjectsInstantiation(int selectedTab)
        {
            var availableJobSources = _playerProfile.GetActiveJobsModule().ActiveJobObjects;
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

        private void ManageSuppliersInstantiation(int selectedTab)
        {
            var availableItemSuppliers = _playerProfile.GetActiveItemSuppliersModule().ActiveItemStores;
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
        
        //TODO: News should be managed with Day Range and should also check for special News Unlocked
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

        private void UpdateItemsContent(int verticalTabIndex)
        {
            if (_playerProfile == null)
            {
                _playerProfile = GameDirector.Instance.GetActiveGameProfile;
            }
            ClearNotebookContent();
            UpdateContentInPage(_mCurrentHorizontalSource, verticalTabIndex);
        }

        public int ActiveTab { get; }
    }
}