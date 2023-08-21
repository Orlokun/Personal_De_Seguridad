using Scripts.ItemPlacement;
using UI.Items;
using UnityEngine.EventSystems;
using Utils;

namespace ItemPlacement
{
    public class SelectCameraTypeItem : BaseSelectItemForPlacement, IInitialize
    {
        private bool _mInitiliazed;
        private IUITabItemObject _mCameraObject;
        public IUITabItemObject CameraObject => _mCameraObject;
        public void SetCameraObject(IUITabItemObject cameraObjectInterface)
        {
            _mCameraObject = cameraObjectInterface;
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
            MInstantiatedObject=Instantiate(_mCameraObject.GetPrefabObject);
            MInstantiatedObject.SetActive(false);
        }
    
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            CameraPlacementManager.Instance.AttachNewObject(MInstantiatedObject);    
        }
    
        //Check Mouse Down Event the mouse down interface
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            //Pass the object that needs to be instantiated to the manager
            CameraPlacementManager.Instance.AttachNewObject(MInstantiatedObject);
        }
        public bool IsInitialized => _mInitiliazed;

    }
}