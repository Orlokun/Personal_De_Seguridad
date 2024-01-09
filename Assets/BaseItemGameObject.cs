using ExternalAssets._3DFOV.Scripts;
using UnityEngine;

public interface IHasFieldOfView
{
    public bool HasfieldOfView { get; }
    public FieldOfView3D FieldOfView3D { get; }
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