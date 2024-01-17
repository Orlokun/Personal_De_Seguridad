using UnityEngine;

public interface IItemCameraRotation : IToggleComponent
{
    public void SetNewPosition(Vector3 newPosition);
}
public interface IToggleComponent
{
    public void ToggleComponentActive(bool isActive);
}
public class ItemCameraRotation : MonoBehaviour, IItemCameraRotation
{
    private Vector3 stablePosition;
    private bool isStatic;

    // Update is called once per frame
    void Update()
    {
        if (isStatic)
        {
            transform.position = stablePosition;
        }
    }

    public void ToggleComponentActive(bool isActive)
    {
        isStatic = isActive;
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        stablePosition = newPosition;
    }
}
