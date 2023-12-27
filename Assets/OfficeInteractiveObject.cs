using UnityEngine;

public interface IInteractiveClickableObject 
{
    public void SendClickObject();
}

public interface IOfficeInteractiveObject : IInteractiveClickableObject
{
    
}

public class OfficeInteractiveObject : MonoBehaviour, IOfficeInteractiveObject
{
    public virtual void SendClickObject()
    {
        Debug.Log($"Object clicked: {gameObject.name}!!!");
    }
}
