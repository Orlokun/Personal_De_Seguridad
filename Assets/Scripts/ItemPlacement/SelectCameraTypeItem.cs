using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace ItemPlacement
{
    public class SelectCameraTypeItem : BaseSelectItemForPlacement, IInitialize
    {
        private bool _mInitiliazed;
        //Prefabs that need to be instantiated
        public GameObject instantiatedPrefab;
        

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
    
        public override void OnPointerClick(PointerEventData eventData)
        {
            CameraPlacementManager.Instance.AttachNewObject(MInstantiatedObject);    
            base.OnPointerClick(eventData);
        }
    
        //Check Mouse Down Event the mouse down interface
        public override void OnPointerDown(PointerEventData eventData)
        {
            CameraPlacementManager.Instance.AttachNewObject(MInstantiatedObject);
            base.OnPointerDown(eventData);
        }
        public bool IsInitialized => _mInitiliazed;

    }
}