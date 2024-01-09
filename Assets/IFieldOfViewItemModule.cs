using ExternalAssets._3DFOV.Scripts;

public interface IFieldOfViewItemModule
{
    public void ToggleInGameFoV(bool isActive);
    public IFieldOfView3D Fov3D { get; }
    public bool TargetsInRange { get; }
}