using UnityEngine;

public interface IOfficeInteractiveObject
{
    public void SendClickObject();
}

public class OfficeInteractiveObject : MonoBehaviour, IOfficeInteractiveObject
{
    public virtual void SendClickObject()
    {
        Debug.Log($"Object clicked: {gameObject.name}!!!");
    }
}
