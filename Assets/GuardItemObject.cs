using Cinemachine;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;
using Utils;

public interface IGuardItemObject : IHasFieldOfView
{

}

public class GuardItemObject : BaseItemGameObject, IGuardItemObject
{
    private CinemachineVirtualCamera myVc;
    public CinemachineVirtualCamera VirtualCamera => myVc;

    public bool HasfieldOfView { get; }
    public FieldOfView3D FieldOfView3D => _fieldOfViewModule.Fov3D;
    
    [SerializeField] private DrawFoVLines _myDrawFieldOfView;
    [SerializeField] private FieldOfView3D _my3dFieldOfView;

    private IFieldOfViewItemModule _fieldOfViewModule;

    private void Awake()
    {
        InPlacement = false;
        ProcessFieldOfView();
    }
    private void ProcessFieldOfView()
    {
        _fieldOfViewModule = Factory.CreateFieldOfViewItemModule(_myDrawFieldOfView, _my3dFieldOfView); 
    }
    public override void SendClickObject()
    {
        Debug.Log($"[CameraItemPrefab.SendClickObject] Clicked object named{gameObject.name}");
        if (InPlacement)
        {
            return;
        }
        _fieldOfViewModule.ToggleInGameFoV(!_my3dFieldOfView.IsDrawActive);
    }


}