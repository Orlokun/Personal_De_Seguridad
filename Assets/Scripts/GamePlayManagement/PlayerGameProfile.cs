using System;
using System.Collections.Generic;
using System.Linq;
using GameManagement;
using GameManagement.ProfileDataModules.ItemStores.StoreInterfaces;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemStores;
using UI;
using UnityEngine;
using Utils;

namespace GamePlayManagement
{
    public class PlayerGameProfile : IPlayerGameProfile
    {
        //Profile constructor
        public PlayerGameProfile(IGeneralItemSources generalItemSources)
        {
            _mGameCreationDate = DateTime.Now;
            _mGameId = Guid.NewGuid();
            _generalItemSources = generalItemSources;
            PopulateItemSourcesDictionary();
        }
        
        //Main Data Modules
        private IGeneralItemSources _generalItemSources;
        private IJobsSourcesModule JobsSourcesModule;

        //Memebers
        private DateTime _mGameCreationDate;
        private Guid _mGameId;
        private Dictionary<int, IItemSourceModule> _mIndexedSourceModules;

        //Public Module Interfaces Fields
        public IItemSourceModule GetActiveItemsInModule(BitItemType value)
        {
            return GetSourceModuleWithBitIndex((int)value);
        }
        public IGuardSourcesModule GuardSourcesModule => _generalItemSources.GuardSourcesModule;
        public ICameraSourcesModule CameraSourcesModule => _generalItemSources.CameraSourcesModule;
        public IWeaponsSourcesModule WeaponSourcesModule => _generalItemSources.WeaponSourcesModule;
        public ITrapSourcesModule TrapSourcesModule => _generalItemSources.TrapSourcesModule;
        public IOtherItemsSourcesModule OtherItemsSourcesModule => _generalItemSources.OtherItemsSourcesModule;


        private void PopulateItemSourcesDictionary()
        {
            var bitMaxIndex = BitOperator.TurnCountIntoMaxBitValue(5);
            _mIndexedSourceModules = new Dictionary<int, IItemSourceModule>();
            for (var i = 1; i < bitMaxIndex; i *= 2)
            {
                var itemSource = (BitItemType) i;
                switch (itemSource)
                {
                    case BitItemType.GUARD_ITEM_TYPE:
                        _mIndexedSourceModules.Add(i, GuardSourcesModule);
                        break;
                    case BitItemType.CAMERA_ITEM_TYPE:
                        _mIndexedSourceModules.Add(i,CameraSourcesModule);
                        break;
                    case BitItemType.WEAPON_ITEM_TYPE:
                        _mIndexedSourceModules.Add(i,WeaponSourcesModule);
                        break;
                    case BitItemType.TRAP_ITEM_TYPE:
                        _mIndexedSourceModules.Add(i,TrapSourcesModule);
                        break;
                    case BitItemType.OTHERS_ITEM_TYPE:
                        _mIndexedSourceModules.Add(i,OtherItemsSourcesModule);
                        break;
                    default:
                        Debug.LogError("Cast into Bit Item Type is not correct. Wrong value i = {i}");
                        break;
                }
            }

            //TODO: Move/Delete Hardcoded Number
            if (_mIndexedSourceModules.Count != 5)
            {
                Debug.LogError("[PopulateItemSourcesDictionary] Population of Item sources interfaces was not properly set!");
            }
        }
        private IItemSourceModule GetSourceModuleWithBitIndex(int bitIndex)
        {
            return _mIndexedSourceModules.SingleOrDefault(x => x.Key == bitIndex).Value;
        }
    }
}