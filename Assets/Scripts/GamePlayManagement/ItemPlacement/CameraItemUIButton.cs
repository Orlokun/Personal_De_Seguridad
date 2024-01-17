using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemManagement;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemPlacement
{
    public class CameraItemUIButton : BaseItemPlacement, IInitialize
    {
        private bool _mInitiliazed;
        //Prefabs that need to be instantiated
        public GameObject instantiatedPrefab;
        public bool IsInitialized => _mInitiliazed;

        protected void Start()
        {
            if (instantiatedPrefab == null)
            {
                return;
            }
            Initialize();
        }

        public void Initialize()
        {
            if (_mInitiliazed)
            {
                return;
            }
            
            CreateAndDeactivatePrefab();
            _mInitiliazed = true;
        }

        private void CreateAndDeactivatePrefab()
        {
            //Instantiate prefab
            MInstantiatedObject=Instantiate(instantiatedPrefab);
            MInstantiatedObject.SetActive(false);
            var objectData = (IBaseItemObject)MInstantiatedObject.GetComponent<CameraItemBaseObject>();
            objectData.SetInPlacementStatus(true);
        }
    
        public override void OnItemClicked(IItemObject itemData)
        {
            base.OnItemClicked(itemData);
            CameraPlacementManager.Instance.AttachNewObject(itemData, MInstantiatedObject);    
            CameraPlacementManager.Instance.ToggleRoofObject(true);    
        }
    }
}