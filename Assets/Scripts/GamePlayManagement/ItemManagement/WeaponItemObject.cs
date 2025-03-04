using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.ItemManagement
{
    public class WeaponItemObject : BaseItemGameObject, IWeaponItemObject
    {
        private IItemObject _mItemData;
        private IWeaponBaseData _mIWeaponBaseDataData;
    }

    public interface IWeaponItemObject
    {
    }
    
    public class TrapItemObject : BaseItemGameObject, ITrapItemObejct
    {
        private IItemObject _mItemData;
    }

    public interface ITrapItemObejct
    {
    }
    
    public class OtherTypeItemObject : BaseItemGameObject, IOtherTypeObject
    {
        private IItemObject _mItemData;
    }

    public interface IOtherTypeObject
    {
    }
}