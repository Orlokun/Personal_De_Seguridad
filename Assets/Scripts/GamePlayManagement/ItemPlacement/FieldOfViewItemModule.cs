using ExternalAssets._3DFOV.Scripts;

namespace GamePlayManagement.ItemPlacement
{
    public class FieldOfViewItemModule : IFieldOfViewItemModule
    {
        private DrawFoVLines _myDrawFieldOfView;
        private IFieldOfView3D _my3dFieldOfView;
        public IFieldOfView3D Fov3D => _my3dFieldOfView;
        public bool TargetsInRange => _my3dFieldOfView.HasTargetsInRange;
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
}