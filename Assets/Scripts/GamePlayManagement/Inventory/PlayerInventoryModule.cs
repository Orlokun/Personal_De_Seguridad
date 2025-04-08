using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Inventory
{
    public class PlayerInventoryModule : IPlayerInventoryModule
    {
        private List<IGuardInInventory> _mGuardItems = new List<IGuardInInventory>();
        private List<ICameraInInventory> _mCameraItems = new List<ICameraInInventory>();
        private List<IWeaponInInventory> _mWeaponItems = new List<IWeaponInInventory>();
        private List<ITrapInInventory> _mTrapItems = new List<ITrapInInventory>();
        private List<IOtherItemInInventory> _mOtherItems = new List<IOtherItemInInventory>();
        
        IPlayerGameProfile _mPlayerProfile;
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mPlayerProfile = currentPlayerProfile;
        }

        public void PlayerLostResetData()
        {
            CleanInventory(); 
        }

        #region AddItem

        public int GetItemCount(BitItemSupplier itemSupplier, int itemId, BitItemType itemType)
        {
            switch (itemType)
            {
                case BitItemType.GUARD_ITEM_TYPE:
                    return _mGuardItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier) ? 
                        _mGuardItems.First(x => x.ItemId == itemId).AvailableCount : 0;
                case BitItemType.CAMERA_ITEM_TYPE:
                    return _mCameraItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier) ?
                        _mCameraItems.First(x => x.ItemId == itemId).AvailableCount : 0;
                case BitItemType.WEAPON_ITEM_TYPE:
                    return _mWeaponItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier) ?
                        _mWeaponItems.First(x => x.ItemId == itemId).AvailableCount : 0;
                case BitItemType.TRAP_ITEM_TYPE:
                    return _mTrapItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier) ?
                        _mTrapItems.First(x => x.ItemId == itemId).AvailableCount : 0;
                case BitItemType.OTHERS_ITEM_TYPE:
                    return _mOtherItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier) ?
                        _mOtherItems.First(x => x.ItemId == itemId).AvailableCount : 0;
                default:
                    return 0;
            }
        }

        public int GetItemCount(BitItemSupplier itemSupplier, int itemId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsItemInInventory(BitItemSupplier itemSupplier, int itemId, BitItemType itemType)
        {
            switch (itemType)
            {
                case BitItemType.GUARD_ITEM_TYPE:
                    return _mGuardItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier && x.AvailableCount > 0);
                case BitItemType.CAMERA_ITEM_TYPE:
                    return _mCameraItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier && x.AvailableCount > 0);
                case BitItemType.WEAPON_ITEM_TYPE:
                    return _mWeaponItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier && x.AvailableCount > 0);
                case BitItemType.TRAP_ITEM_TYPE:
                    return _mTrapItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier && x.AvailableCount > 0);
                case BitItemType.OTHERS_ITEM_TYPE:
                    return _mOtherItems.Any(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier && x.AvailableCount > 0);
                default:
                    return false;
            }
        }

        public void AddItemToInventory(IItemObject incomingItem, int amount)
        {
            switch (incomingItem.ItemType)
            {
                case BitItemType.GUARD_ITEM_TYPE:
                    ProcessAddedGuardItem(incomingItem, amount);
                    break;
                case BitItemType.CAMERA_ITEM_TYPE:
                    ProcessAddedCameraItem(incomingItem, amount);
                    break;
                case BitItemType.WEAPON_ITEM_TYPE:
                    ProcessAddedWeaponItem(incomingItem, amount);
                    break;
                case BitItemType.TRAP_ITEM_TYPE:
                    ProcessAddedTrapItem(incomingItem, amount);
                    break;
                case BitItemType.OTHERS_ITEM_TYPE:
                    ProcessAddedOtherTypeItem(incomingItem, amount);
                    break;
            }
        }

        public List<IItemInInventory> GetItemsOfType(BitItemType itemType)
        {
            switch (itemType)
            {   
                case BitItemType.GUARD_ITEM_TYPE:
                    return _mGuardItems.Cast<IItemInInventory>().ToList();
                case BitItemType.CAMERA_ITEM_TYPE:
                    return _mCameraItems.Cast<IItemInInventory>().ToList();
                case BitItemType.WEAPON_ITEM_TYPE:
                    return _mWeaponItems.Cast<IItemInInventory>().ToList();
                case BitItemType.TRAP_ITEM_TYPE:
                    return _mTrapItems.Cast<IItemInInventory>().ToList();
                case BitItemType.OTHERS_ITEM_TYPE:
                    return _mOtherItems.Cast<IItemInInventory>().ToList();
                default:
                    return null;
            }
        }

        public void ClearItemFromInventory(BitItemSupplier itemSupplier, int itemId)
        {
            _mGuardItems.RemoveAll(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier);
            _mCameraItems.RemoveAll(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier);
            _mWeaponItems.RemoveAll(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier);
            _mTrapItems.RemoveAll(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier);
            _mOtherItems.RemoveAll(x => x.ItemId == itemId && x.ItemSupplier == itemSupplier);
        }

        public void CleanInventory()
        {
            _mGuardItems.Clear();
            _mCameraItems.Clear();
            _mWeaponItems.Clear();
            _mTrapItems.Clear();
            _mOtherItems.Clear();
        }

        public List<IGuardInInventory> GetGuardItems => _mGuardItems;
        public List<ICameraInInventory> GetCameraItems => _mCameraItems;
        public List<IWeaponInInventory> GetWeaponItems => _mWeaponItems;
        public List<ITrapInInventory> GetTrapItems => _mTrapItems;
        public List<IOtherItemInInventory> GetOtherItems => _mOtherItems;

        private void ProcessAddedGuardItem(IItemObject incomingItem, int amount)
        {
            if (_mGuardItems.Any(x=> x.ItemId == incomingItem.BitId && x.ItemSupplier == incomingItem.ItemSupplier))
            {
                _mGuardItems.First(x => x.ItemId == incomingItem.BitId).AddToInventory(amount);
            }
            else
            {
                GuardInInventory newIGuard = new GuardInInventory(incomingItem);
                newIGuard.AddToInventory(amount);
                _mGuardItems.Add(newIGuard);
            }
        }
        private void ProcessAddedCameraItem(IItemObject incomingItem, int amount)
        {
            if (_mCameraItems.Any(x=> x.ItemId == incomingItem.BitId && x.ItemSupplier == incomingItem.ItemSupplier))
            {
                _mCameraItems.First(x => x.ItemId == incomingItem.BitId).AddToInventory(amount);
            }
            else
            {
                ICameraInInventory newCamera = new CameraInInventory(incomingItem);
                newCamera.AddToInventory(amount);
                _mCameraItems.Add(newCamera);
            }
        }
        private void ProcessAddedWeaponItem(IItemObject incomingItem, int amount)
        {
            if (_mWeaponItems.Any(x=> x.ItemId == incomingItem.BitId && x.ItemSupplier == incomingItem.ItemSupplier))
            {
                _mWeaponItems.First(x => x.ItemId == incomingItem.BitId).AddToInventory(amount);
            }
            else
            {
                IWeaponInInventory newWeapon = new WeaponInInventory(incomingItem);
                newWeapon.AddToInventory(amount);
                _mWeaponItems.Add(newWeapon);
            }
        }
        private void ProcessAddedTrapItem(IItemObject incomingItem, int amount)
        {
            if (_mTrapItems.Any(x=> x.ItemId == incomingItem.BitId && x.ItemSupplier == incomingItem.ItemSupplier))
            {
                _mTrapItems.First(x => x.ItemId == incomingItem.BitId).AddToInventory(amount);
            }
            else
            {
                ITrapInInventory newTrap = new TrapInInventory(incomingItem);
                newTrap.AddToInventory(amount);
                _mTrapItems.Add(newTrap);
            }
        }
        private void ProcessAddedOtherTypeItem(IItemObject incomingItem, int amount)
        {
            if (_mOtherItems.Any(x=> x.ItemId == incomingItem.BitId && x.ItemSupplier == incomingItem.ItemSupplier))
            {
                _mOtherItems.First(x => x.ItemId == incomingItem.BitId).AddToInventory(amount);
            }
            else
            {
                IOtherItemInInventory newOtherItem = new OtherItemInInventory(incomingItem);
                newOtherItem.AddToInventory(amount);
                _mOtherItems.Add(newOtherItem);
            }
        }
        #endregion
    }
}