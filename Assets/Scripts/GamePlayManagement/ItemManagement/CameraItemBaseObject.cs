using Cinemachine;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;
using Utils;

namespace GamePlayManagement.ItemManagement
{
    public class CameraItemBaseObject : BaseItemGameObject, IHasFieldOfView, IInteractiveClickableObject
    {
        private CinemachineVirtualCamera myVc;
        public CinemachineVirtualCamera VirtualCamera => myVc;

        private IFieldOfViewItemModule _fieldOfViewModule;
        [SerializeField]private DrawFoVLines _myDrawFieldOfView;
        [SerializeField]private FieldOfView3D _my3dFieldOfView;
    
        public bool HasFieldOfView { get; }
        public IFieldOfView3D FieldOfView3D { get; }
    
        private void Awake()
        {
            InPlacement = false;
            ProcessFieldOfView();
        }
        private void ProcessFieldOfView()
        {
            _myDrawFieldOfView = GetComponent<DrawFoVLines>();
            _my3dFieldOfView = GetComponent<FieldOfView3D>();
            _fieldOfViewModule = Factory.CreateFieldOfViewItemModule(_myDrawFieldOfView, _my3dFieldOfView); 
        }
        #region Interactive Object Interface
        public override void ReceiveSelectClickEvent()
        {
            Debug.Log($"[CameraItemPrefab.SendClickObject] Clicked object named{gameObject.name}");
            if (InPlacement)
            {
                return;
            }
            _fieldOfViewModule.ToggleInGameFoV(!_my3dFieldOfView.IsDrawActive);
        }
        public void ReceiveDeselectObjectEvent()
        {
            
        }
        public void ReceiveActionClickedEvent()
        {
            
        }
        #endregion
    }        
}