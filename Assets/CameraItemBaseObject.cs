using Cinemachine;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;

public interface IBaseItemObject : IInteractiveClickableObject
{
    public bool IsInPlacement { get; }
    public void SetInPlacementStatus(bool inPlacement);
}

public abstract class BaseItemGameObject : MonoBehaviour, IBaseItemObject
{
    protected bool InPlacement;
    public bool IsInPlacement => InPlacement;

    public virtual void SendClickObject()
    {
    }
    public void SetInPlacementStatus(bool inPlacement)
    {
        InPlacement = inPlacement;
    }

    private bool hasSnippet = false;
    public bool HasSnippet => hasSnippet;

    public string GetSnippetText { get; }

    public void DisplaySnippet()
    {
        
    }
}

public class CameraItemBaseObject : BaseItemGameObject, IBaseItemObject
{
    private CinemachineVirtualCamera myVc;
    public CinemachineVirtualCamera VirtualCamera => myVc;

    private DrawFoVLines _myDrawFieldOfView;
    private FieldOfView3D _my3dFieldOfView;
    private void Awake()
    {
        InPlacement = false;
        _myDrawFieldOfView = GetComponent<DrawFoVLines>();
        _my3dFieldOfView = GetComponent<FieldOfView3D>();
        
        if (_myDrawFieldOfView == null)
        {
            Debug.LogError("Camera Item must have a Draw Field of View available");
        }
        if (_my3dFieldOfView == null)
        {
            Debug.LogError("Camera Item must have a Draw Field of View available");
        }
    }

    public override void SendClickObject()
    {
        Debug.Log($"[CameraItemPrefab.SendClickObject] Clicked object named{gameObject.name}");
        if (InPlacement)
        {
            return;
        }
        _my3dFieldOfView.ToggleInGameFoV(!_my3dFieldOfView.IsDrawActive);
    }
}
