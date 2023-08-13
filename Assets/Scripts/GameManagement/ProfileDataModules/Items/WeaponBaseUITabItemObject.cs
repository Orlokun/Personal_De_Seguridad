using GameManagement.BitDescriptions;
using GameManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GameManagement.ProfileDataModules.Items
{
    [CreateAssetMenu(menuName = "Items/WeaponItem")]
    public class WeaponBaseUITabItemObject : BaseUITabItemObject
    {
        [SerializeField] protected BitWeaponSupplier weaponSupplier;
        [SerializeField] protected BitWeaponItems weaponBitKey;
    }
}