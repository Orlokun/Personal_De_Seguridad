using DataUnits.ItemScriptableObjects;
using InputManagement.MouseInput;
using Utils;

namespace GamePlayManagement.ItemManagement
{
    public interface ICameraItemBaseObject : IBaseItemObject, IHasFieldOfView, IInteractiveClickableObject, IInitializeWithArg1<IItemObject>
    {
        
    }
}