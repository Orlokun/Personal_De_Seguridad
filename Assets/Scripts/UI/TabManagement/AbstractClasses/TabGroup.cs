using System;
using System.Collections.Generic;
using UI.TabManagement.Interfaces;
using UnityEngine;
using Utils;

namespace UI.TabManagement.AbstractClasses
{
    public abstract class TabGroup : MonoBehaviour, ITabGroup, ITabUpdate
    {
        [SerializeField] protected List<TabElement> tabElements;

        protected Dictionary<int, ITabElement> MTabElements = new Dictionary<int, ITabElement>();
        protected bool MIsTabActive;
        protected int MActiveTab = 1;
        protected IUIController MuiController;

        public int ActiveTab => MActiveTab;
        public bool IsTabGroupActive => MIsTabActive;

        public virtual bool ActivateTabInUI()
        {
            var e = new Exception("[TabGroup.ActivateGroup] Abstract method must be overriden by inheritor class");
            throw e;
        }

        public virtual bool DeactivateGroupInUI()
        {
            var e = new Exception("[TabGroup.DeactivateGroup] Abstract method must be overriden by inheritor class");
            throw e;
        }

        public void UpdateDictionaryData()
        {
            InitializeTabElementsDataDict();
        }

        protected virtual void Awake()
        {
            InitializeTabElementsDataDict();
        }
        protected virtual void Start()
        {
            MuiController = UIController.Instance;
            MuiController.OnResetCanvas += OnResetCanvas;
        }

        protected virtual void OnResetCanvas()
        {
            //var isActive = DeactivateGroupInUI();
            //Debug.Log($"[TabGroup] On Event Deactivate: Is active = {isActive}");
        }

        private void InitializeTabElementsDataDict()
        {
            if (tabElements.Count == 0)
            {
                return;
            }
            
            MTabElements = new Dictionary<int, ITabElement>();
            var bitLimit = BitOperator.TurnCountIntoMaxBitValue(tabElements.Count);

            var listIndex = 0;
            for (var i = 1; i< bitLimit; i*=2)
            {
                MTabElements.Add(i, tabElements[listIndex]);
                tabElements[listIndex].SetTabIndex(i);
                tabElements[listIndex].Initialize(this);
                listIndex++;
            }
        }

        public virtual void UpdateItemsContent(int selectedTabIndex, int verticalTabIndex)
        {
            MActiveTab = selectedTabIndex;
        }
    }
}