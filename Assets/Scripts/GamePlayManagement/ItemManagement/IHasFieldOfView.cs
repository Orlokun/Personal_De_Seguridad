using ExternalAssets._3DFOV.Scripts;

namespace GamePlayManagement.ItemManagement
{
    public interface IHasFieldOfView
    {
        public bool HasFieldOfView { get; }
        public IFieldOfView3D FieldOfView3D { get; }
    }
}