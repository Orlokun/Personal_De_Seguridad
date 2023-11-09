using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraItemPrefab : MonoBehaviour, IPointerClickHandler
{
    public bool beingUsed;
    private CinemachineVirtualCamera myVc;
    public CinemachineVirtualCamera VirtualCamera => myVc;
    private void Awake()
    {
        beingUsed = false;
        myVc = GetComponent<CinemachineVirtualCamera>();
        if (myVc == null)
        {
            Debug.LogError("Camera Item must have a camera available");
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked object!");
        if (beingUsed)
        {
            return;
        }
    }
}
