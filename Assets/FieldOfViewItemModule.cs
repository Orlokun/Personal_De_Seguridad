using ExternalAssets._3DFOV.Scripts;
public class FieldOfViewItemModule : IFieldOfViewItemModule
{
    private DrawFoVLines _myDrawFieldOfView;
    private FieldOfView3D _my3dFieldOfView;
    public FieldOfView3D Fov3D => _my3dFieldOfView;
    public DrawFoVLines DrawFieldOfView => _myDrawFieldOfView;
    
    
    private bool _mIsInitialized;
    public bool IsInitialized => _mIsInitialized;

    public FieldOfViewItemModule(DrawFoVLines dFov, FieldOfView3D fov3D)
    {
        _myDrawFieldOfView = dFov;
        _my3dFieldOfView = fov3D;
        _mIsInitialized = true;
    }

    public void ToggleInGameFoV(bool isActive)
    {
        _my3dFieldOfView.ToggleInGameFoV(isActive);
    }


}