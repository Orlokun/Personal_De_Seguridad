using ExternalAssets._3DFOV.Scripts;

public interface IFieldOfViewItemModule
{
    public void ToggleInGameFoV(bool isActive);
    public FieldOfView3D Fov3D { get; }
}