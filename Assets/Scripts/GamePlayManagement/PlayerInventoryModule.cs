using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;

namespace GamePlayManagement
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
            _mGuardItems.Clear(); 
            _mCameraItems.Clear();
            _mWeaponItems.Clear();
            _mTrapItems.Clear(); 
            _mOtherItems.Clear();        
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

        private void ProcessAddedGuardItem(IItemObject incomingItem, int amount)
        {
            if (_mGuardItems.Any(x=> x.ItemId == incomingItem.BitId && x.ItemSupplier == incomingItem.ItemSupplier))
            {
                _mGuardItems.First(x => x.ItemId == incomingItem.BitId).AddToInventory(amount);
            }
            else
            {
                GuardInInventory newIGuard = new GuardInInventory();
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
                ICameraInInventory newCamera = new CameraInInventory();
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
                IWeaponInInventory newWeapon = new WeaponInInventory();
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
                ITrapInInventory newTrap = new TrapInInventory();
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
                IOtherItemInInventory newOtherItem = new OtherItemInInventory();
                newOtherItem.AddToInventory(amount);
                _mOtherItems.Add(newOtherItem);
            }
        }
       
        public void RemoveItemFromInventory(IItemObject removedItem, int amount)
        {
            
        }
    }
}