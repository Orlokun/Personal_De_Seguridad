using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using ExternalAssets._3DFOV.Scripts;
using Players_NPC;
using Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using UnityEngine;
using Utils;

public interface IGuardItemObject : IHasFieldOfView
{

}

public class GuardBaseGameObject : BaseItemGameObject, IGuardItemObject
{
    private CinemachineVirtualCamera myVc;
    public CinemachineVirtualCamera VirtualCamera => myVc;

    public bool HasfieldOfView { get; }
    public IFieldOfView3D FieldOfView3D => _fieldOfViewModule.Fov3D;
    
    [SerializeField] private DrawFoVLines _myDrawFieldOfView;
    [SerializeField] private FieldOfView3D _my3dFieldOfView;

    private Dictionary<Guid, IBaseCustomer> _mTrackedCustomers;

    private IFieldOfViewItemModule _fieldOfViewModule;

    private void Awake()
    {
        InPlacement = false;
        ProcessFieldOfView();
    }

    private void ProcessTargetsInView()
    {
        var seenObjects = _fieldOfViewModule.Fov3D.SeenTargetObjects;
        foreach (var seenObject in seenObjects)
        {
            var isCustomer = seenObject.TryGetComponent<IBaseCustomer>(out var customerStatus);
            if (!isCustomer)
            {
                continue;
            }
            if(!_mTrackedCustomers.ContainsKey(customerStatus.CustomerId))
            {
                _mTrackedCustomers.Add(customerStatus.CustomerId, customerStatus);
            }
        }

        var removedCustomers = new List<Guid>();
        foreach (var trackedCustomer in _mTrackedCustomers)
        {
            var customersSeen = seenObjects.Where(x=> x.TryGetComponent<IBaseCustomer>(out var customerData));
            if (customersSeen.All(x => x.GetComponent<IBaseCustomer>().CustomerId != trackedCustomer.Key))
            {
                continue;
            }
            removedCustomers.Add(trackedCustomer.Key);
        }

        foreach (var removedCustomer in removedCustomers)
        {
            if (!_mTrackedCustomers.ContainsKey(removedCustomer))
            {
                Debug.LogError("Tracked Customer must be tracked before being removed of tracking list");
                continue;
            }
            _mTrackedCustomers.Remove(removedCustomer);
        }
        removedCustomers.Clear();
    }
    protected override void ProcessInViewTargets()
    {
        if (!_fieldOfViewModule.Fov3D.HasTargetsInRange)
        {
            return;
        }
        ProcessTargetsInView();
        ProcessCustomersSeen();
    }

    private void ProcessCustomersSeen()
    {
        foreach (var mTrackedCustomer in _mTrackedCustomers)
        {
            mTrackedCustomer.Value.GetCustomerVisitData
        }
    }
    
    private void ProcessFieldOfView()
    {
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