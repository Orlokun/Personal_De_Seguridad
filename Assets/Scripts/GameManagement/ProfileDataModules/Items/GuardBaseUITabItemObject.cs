using GameManagement.BitDescriptions;
using GameManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace GameManagement.ProfileDataModules.Items
{
    [CreateAssetMenu(menuName = "Items/GuardItem")]
    public class GuardBaseUITabItemObject : BaseUITabItemObject
    {
        [SerializeField] protected BitWeaponSupplier weaponSupplier;
        [SerializeField] protected BitWeaponItems weaponBitKey;
    }
}