using DataUnits.ItemScriptableObjects;

namespace GamePlayManagement.ItemManagement
{
    public class WeaponItemObject : BaseItemGameObject, IWeaponItemObject
    {
        private IItemObject _mItemData;
        private IWeaponBaseData _mIWeaponBaseDataData;
    }
}