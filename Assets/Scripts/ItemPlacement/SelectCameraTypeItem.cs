using UnityEngine;
using Utils;

namespace ItemPlacement
{
    public class SelectCameraTypeItem : BaseSelectItemForPlacement, IInitialize
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
        }
    
        public override void OnItemClicked()
        {
            base.OnItemClicked();
            CameraPlacementManager.Instance.AttachNewObject(MInstantiatedObject);    
            CameraPlacementManager.Instance.ToggleRoofObject(true);    
        }
    }
}