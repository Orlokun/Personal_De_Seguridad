using Cinemachine;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;
using Utils;

public class CameraItemBaseObject : BaseItemGameObject, IHasFieldOfView
{
    private CinemachineVirtualCamera myVc;
    public CinemachineVirtualCamera VirtualCamera => myVc;

    private IFieldOfViewItemModule _fieldOfViewModule;
    [SerializeField]private DrawFoVLines _myDrawFieldOfView;
    [SerializeField]private FieldOfView3D _my3dFieldOfView;
    
    public bool HasfieldOfView { get; }
    public FieldOfView3D FieldOfView3D { get; }
    
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