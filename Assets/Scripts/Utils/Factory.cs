using System.Collections.Generic;
using DialogueSystem;
using GameManagement;
using GameManagement.ProfileDataModules.ItemStores.StoreInterfaces;
using GameManagement.ProfileDataModules.ItemStores.Stores;
using GamePlayManagement;
using GamePlayManagement.ProfileDataModules;
using UnityEngine;

namespace Utils
{
    public static class Factory
    {
        public static DialogueCameraMan CreateCameraMan()
        {
            return new DialogueCameraMan();
        }

        public static GuardSourcesModule CreateGuardSourcesModule()
        {
            return new GuardSourcesModule();
        }

        public static CameraSourceModule CreateCameraSourceModule()
        {
            return new CameraSourceModule();
        }

        public static WeaponsSourceModule CreateWeaponsSourceModule()
        {
            return new WeaponsSourceModule();
        }

        public static TrapSourceModule CreateTrapSourceModule()
        {
            return new TrapSourceModule();
        }

        public static OtherItemsSourceModule CreateOtherItemsSourceModule()
        {
            return new OtherItemsSourceModule();
        }

        public static GeneralItemSource CreateGeneralItemSource()
        {
            return new GeneralItemSource();
        }

        public static PlayerGameProfile CreatePlayerGameProfile(GeneralItemSource generalItemSource)
        {
            return new PlayerGameProfile(generalItemSource);
        }

        public static JobsSourceModule CreateJobSourcesModule()
        {
            return new JobsSourceModule();
        }
    }
    public static class CustomTime
    {
        public static float LocalTimeScale = 1;
        public static float deltaTime {
            get {
                return Time.deltaTime * LocalTimeScale;
            }
        }
 
        public static bool IsPaused {
            get {
                return LocalTimeScale == 0f;
            }
        }
 
        public static float TimeScale {
            get {
                return Time.timeScale * LocalTimeScale;
            }
        }
    }
}