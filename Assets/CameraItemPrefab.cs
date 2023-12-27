using Cinemachine;
using ExternalAssets._3DFOV.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraItemPrefab : MonoBehaviour, IInteractiveClickableObject
{
    public bool beingUsed;
    private CinemachineVirtualCamera myVc;
    public CinemachineVirtualCamera VirtualCamera => myVc;

    private DrawFoVLines myDrawFieldOfView;
    private FieldOfView3D my3dFieldOfView;
    private void Awake()
    {
        beingUsed = false;
        myDrawFieldOfView = GetComponent<DrawFoVLines>();
        if (myDrawFieldOfView == null)
        {
            Debug.LogError("Camera Item must have a Draw Field of View available");
        }
    }

    public void SendClickObject()
    {
        Debug.Log($"Clicked object!{gameObject.name}");
    }
}
