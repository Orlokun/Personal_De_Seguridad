using UI.PopUpManager;
using UnityEngine;

public interface IOfficeInteractiveObject
{
    public void SendClickObject();
}

public class CigarAshtrayOfficeObject : OfficeInteractiveObject
{
    public override void SendClickObject()
    {
        PopUpOperator.Instance.ActivatePopUp(BitPopUpId.NOTEBOOK_ACTION_POPUP);
    }
}

public class OfficeInteractiveObject : MonoBehaviour, IOfficeInteractiveObject
{
    public virtual void SendClickObject()
    {
        Debug.Log($"Object clicked: {gameObject.name}!!!");
    }
}
