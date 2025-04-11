using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;
using GamePlayManagement.BitDescriptions.Suppliers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.GameCatalogues
{
    public class ItemsDataController : MonoBehaviour, IItemsDataController
    {
        private static ItemsDataController _instance;
        public static IItemsDataController Instance => _instance;
        public bool AreSpecialStatsReady => MAreSpecialStatsReady;

        #region Special Stats Load Flags
        private bool MAreSpecialStatsReady => _mGotGuardsData && _mGotCamerasData && _mGotWeaponsData &&
                                              _mGotTrapsData && _mGotOtherItemsData && _mBaseDataIsReady;
        private bool _mGotGuardsData;
        private bool _mGotCamerasData;
        private bool _mGotWeaponsData;
        private bool _mGotTrapsData;
        private bool _mGotOtherItemsData;
        private bool _mBaseDataIsReady;

        #endregion

        #region Special Stats Members
        private ItemCatalogueFromData _mItemCatalogueDataString;
        private GuardStatsFromData _mGuardStatsFromDataString;
        private CameraStatsFromData _mCameraStatsFromDataString;
        private WeaponStatsFromData _mWeaponStatsFromDataString;
        private TrapStatsFromData _mTrapStatsFromDataString;
        private OtherItemsStatsFromData _mOtherItemsStatsFromDataString;
        
        /// <summary>
        /// Dictionary with all possible item data for each supplier
        /// </summary>
        private Dictionary<BitItemSupplier, List<IGuardBaseData>> _mGuardsSpecialData = new Dictionary<BitItemSupplier, List<IGuardBaseData>>();
        private Dictionary<BitItemSupplier, List<ICameraBaseData>> _mCamerasSpecialData = new Dictionary<BitItemSupplier, List<ICameraBaseData>>();
        private Dictionary<BitItemSupplier, List<IWeaponBaseData>> _mWeaponsSpecialData = new Dictionary<BitItemSupplier, List<IWeaponBaseData>>();
        private Dictionary<BitItemSupplier, List<ITrapBaseData>> _mTrapsSpecialData = new Dictionary<BitItemSupplier, List<ITrapBaseData>>();
        private Dictionary<BitItemSupplier, List<IOtherItemBaseData>> _mOthersSpecialData = new Dictionary<BitItemSupplier, List<IOtherItemBaseData>>();
        #endregion
        public Dictionary<BitItemSupplier, List<IItemObject>> ExistingBaseItemsInCatalogue => _mBaseCatalogueBaseItemsFromData;
        private Dictionary<BitItemSupplier, List<IItemObject>> _mBaseCatalogueBaseItemsFromData= new Dictionary<BitItemSupplier, List<IItemObject>>();
        private bool _mInjectedData;

        public List<IItemObject> GetAllSupplierItems(BitItemSupplier suppliers)
        {
            return _mBaseCatalogueBaseItemsFromData[suppliers];
        }

        public IItemObject GetItemFromBaseCatalogue(BitItemSupplier itemSupplier, int itemBitId)
        {
            return _mBaseCatalogueBaseItemsFromData[itemSupplier].First(x => x.BitId == itemBitId);
        }


        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null)
            {
                Destroy(this);
            }
            _instance = this;
            var generalDataUrl = DataSheetUrls.ItemsCatalogueGameData;
            StartCoroutine(LoadItemsCatalogueData(generalDataUrl));
            GetItemsCatalogueData();
        }
        private void GetItemsCatalogueData()
        {
            Debug.Log($"[GetItemsCatalogueData] START: collecting Item types special data");
            var guardsSpecialDataUrl = DataSheetUrls.GuardsSpecialStats;
            var camerasSpecialDataUrl = DataSheetUrls.CameraSpecialStats;
            var weaponsSpecialDataUrl = DataSheetUrls.WeaponsSpecialStats;
            var trapsSpecialDataUrl = DataSheetUrls.TrapsSpecialStats;
            var otherItemsSpecialDataUrl = DataSheetUrls.OtherItemsSpecialStats;

            //1. Load Special Stats for each type of ite
            StartCoroutine(GetGuardSpecialData(guardsSpecialDataUrl));
            StartCoroutine(GetCamerasSpecialData(camerasSpecialDataUrl));
            StartCoroutine(GetWeaponSpecialData(weaponsSpecialDataUrl));
            StartCoroutine(GetTrapSpecialData(trapsSpecialDataUrl));
            StartCoroutine(GetOtherItemsSpecialData(otherItemsSpecialDataUrl));
            StartCoroutine(UpdateSpecialData());
            //
        }

        private IEnumerator UpdateSpecialData()
        {
            yield return new WaitUntil(() => MAreSpecialStatsReady);
            Debug.Log($"[ItemsDataController.UpdateSpecialData] Base data and Special Stats are ready. Injecting Stats to data.");
            InjectItemSpecialStats();
        }

        private void Start()
        {

        }

        #region GetDataFromSheet
        private IEnumerator GetGuardSpecialData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadGuardDataFromJson(sourceJson);
            }
        }
        private IEnumerator GetCamerasSpecialData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadCameraItemsDataFromJson(sourceJson);
            }
        }
        private IEnumerator GetWeaponSpecialData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadWeaponDataFromJson(sourceJson);
            }
        }
        private IEnumerator GetTrapSpecialData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadTrapDataFromJson(sourceJson);
            }
        }
        private IEnumerator GetOtherItemsSpecialData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadOtherItemsFromJson(sourceJson);
            }
        }
        private IEnumerator LoadItemsCatalogueData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("GetItems Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadBaseItemDataFromJson(sourceJson);
            }
        }
        #endregion

        #region Parse Jsons

        #region SpecialStatsJsonParse
        private void LoadGuardDataFromJson(string sourceJson)
        {
            Debug.Log($"[ItemsDataController.LoadGuardDataFromJson]");
            _mGuardStatsFromDataString = JsonConvert.DeserializeObject<GuardStatsFromData>(sourceJson);
            _mGuardsSpecialData = new Dictionary<BitItemSupplier, List<IGuardBaseData>>();
            
            var lastItemSupplierId = 1;
            var currentSupplierGuardsList = new List<IGuardBaseData>();
            if (_mGuardStatsFromDataString == null)
            {
                Debug.LogError("[LoadGuardDataFromJson] Guard Stats data must be reachable");
                return;
            }
            
            for (var i = 1; i < _mGuardStatsFromDataString.values.Count; i++)
            {
                //Step 1: Get Item supplier Id
                var gotId = int.TryParse(_mGuardStatsFromDataString.values[i][0], out var supplierId);
                if (!gotId)
                {
                    Debug.LogError("Item must have a supplier set from data");
                    return;
                }
                var supplierBitId = (BitItemSupplier) supplierId;
                //Step 1.1: If its new supplier, add current list to Dict and clean.
                if (lastItemSupplierId != supplierId || i == 1)
                {
                    _mGuardsSpecialData.Add(supplierBitId, currentSupplierGuardsList);
                    currentSupplierGuardsList = new List<IGuardBaseData>();
                    lastItemSupplierId = supplierId;
                }

                //Step 2: Get Item ID
                var gotItemId = int.TryParse(_mGuardStatsFromDataString.values[i][1], out var itemId);
                if (!gotItemId)
                {
                    Debug.LogError("Item must have a BitID set from data");
                    return;
                }
                
                //Step 3: Get Intelligence
                var gotIntelligence = int.TryParse(_mGuardStatsFromDataString.values[i][3], out var intelligence);
                if (!gotIntelligence)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Intelligence must be available in data");
                }
                //Step 4: Get Kindness
                var gotKindness = int.TryParse(_mGuardStatsFromDataString.values[i][4], out var kindness);
                if (!gotKindness)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Kindness must be available in data");
                }
                //Step 5: Get Proactivity
                var gotProactivity = int.TryParse(_mGuardStatsFromDataString.values[i][5], out var proactivity);
                if (!gotProactivity)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Proactivity must be available in data");
                }
                //Step 6: Get Aggressive
                var gotAggressive = int.TryParse(_mGuardStatsFromDataString.values[i][6], out var aggressive);
                if (!gotAggressive)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Aggressive must be available in data");
                }
                //Step 7: Get Strength
                var gotStrength = int.TryParse(_mGuardStatsFromDataString.values[i][7], out var strength);
                if (!gotStrength)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Strength must be available in data");
                }
                //Step 8: Get Agility
                var gotAgility = int.TryParse(_mGuardStatsFromDataString.values[i][8], out var agility);
                if (!gotAgility)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Agility must be available in data");
                }
                //Step 10: Get Persuasiveness
                var gotPersuasiveness = int.TryParse(_mGuardStatsFromDataString.values[i][9], out var persuasiveness);
                if (!gotPersuasiveness)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Persuasiveness must be available in data");
                }
                
                //Step 10: Get Speed
                var gotSpeed = int.TryParse(_mGuardStatsFromDataString.values[i][10], out var speed);
                if (!gotSpeed)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Speed must be available in data");
                }                
                //Step 11: Get FovRange
                var gotFovRange = int.TryParse(_mGuardStatsFromDataString.values[i][11], out var fov);
                if (!gotSpeed)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard Speed must be available in data");
                }
                
                //Step 12: START COMMON SPECS
                var origin = _mGuardStatsFromDataString.values[i][12];
                var itemType = _mGuardStatsFromDataString.values[i][13];
                var quality = _mGuardStatsFromDataString.values[i][14];
                
                //Step 12.1: Check Origin
                var isOriginValid = Enum.TryParse<ItemOrigin>(origin, out var originValue);
                if (!isOriginValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have origin set");
                    return;
                }
                
                //Step 12.2: Check ItemType
                var itemTypes = itemType.Split(',');
                if(itemTypes.Any(x=> Enum.TryParse<ItemBaseType>(x, out var itemTypeValue) == false))
                {
                    Debug.LogWarning($"[LoadGuardDataFromJson] All Item types set for Guard {supplierBitId} must be valid");
                    return;
                }
                var castItemTypes = itemTypes.Select(x => Enum.Parse<ItemBaseType>(x)).ToList();

                //Step 12.3: Check Item Quality
                var isItemQualityValid = Enum.TryParse<ItemBaseQuality>(quality, out var itemQualityValue);
                if (!isItemQualityValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have item quality set");
                    return;
                }

                IGuardBaseData itemDataObject =
                    new GuardBaseData((BitItemSupplier)supplierId, itemId,intelligence, kindness, proactivity, aggressive, strength, agility, 
                        persuasiveness, speed, fov, originValue, castItemTypes, itemQualityValue);
                _mGuardsSpecialData[supplierBitId].Add(itemDataObject);
            }
            _mGotGuardsData = true;
        }
        private void LoadCameraItemsDataFromJson(string sourceJson)
        {
            Debug.Log($"[ItemsDataController.LoadCameraDataFromJson]");
            _mCameraStatsFromDataString = JsonConvert.DeserializeObject<CameraStatsFromData>(sourceJson);
            _mCamerasSpecialData = new Dictionary<BitItemSupplier, List<ICameraBaseData>>();
            
            var lastItemSupplierId = 1;
            var currentSupplierCameraStats = new List<ICameraBaseData>();
            
            if (_mCameraStatsFromDataString == null)
            {
                Debug.LogError("[LoadCameraItemsDataFromJson] Camera Stats data must be reachable");
                return;
            }
            for (var i = 1; i < _mCameraStatsFromDataString.values.Count; i++)
            {
                //Step 1: Get Item supplier Id
                var gotId = int.TryParse(_mCameraStatsFromDataString.values[i][0], out var supplierId);
                if (!gotId)
                {
                    Debug.LogError("Item must have a supplier set from data");
                    return;
                }
                var supplierBitId = (BitItemSupplier) supplierId;
                //Step 1.1: If its new supplier, add current list to Dict and clean.
                if (lastItemSupplierId != supplierId || i == 1)
                {
                    _mCamerasSpecialData.Add(supplierBitId, currentSupplierCameraStats);
                    currentSupplierCameraStats = new List<ICameraBaseData>();
                    lastItemSupplierId = supplierId;
                }

                //Step 2: Get Item ID
                var gotItemId = int.TryParse(_mCameraStatsFromDataString.values[i][1], out var itemId);
                if (!gotItemId)
                {
                    Debug.LogError("Item must have a BitID set from data");
                    return;
                }
                
                //Step 3: Get Range
                var gotRange = int.TryParse(_mCameraStatsFromDataString.values[i][3], out var range);
                if (!gotRange)
                {
                    Debug.LogWarning("[LoadCameraItemsDataFromJson] Camera Stats must have a range");
                }
                //Step 4: Get People In Sight
                var gotPiS = int.TryParse(_mCameraStatsFromDataString.values[i][4], out var peopleInSight);
                if (!gotPiS)
                {
                    Debug.LogWarning("[LoadCameraItemsDataFromJson] Camera Stats must have a peopleInSight");
                }
                //Step 5: Clarity
                var gotClarity = int.TryParse(_mCameraStatsFromDataString.values[i][5], out var clarity);
                if (!gotClarity)
                {
                    Debug.LogWarning("[LoadCameraItemsDataFromJson] Camera Stats must have a clarity");
                }
                //Step 6: Persuasiveness
                var gotPersuasiveness = int.TryParse(_mCameraStatsFromDataString.values[i][6], out var persuasiveness);
                if (!gotPersuasiveness)
                {
                    Debug.LogWarning("[LoadCameraItemsDataFromJson] Camera Stats must have a persuasiveness");
                }
                
                //Step 7: FoV Range
                var gotFoVRange = int.TryParse(_mCameraStatsFromDataString.values[i][7], out var fovRadius);
                if (!gotFoVRange)
                {
                    Debug.LogWarning("[LoadCameraItemsDataFromJson] Camera Stats must have a fovRadius");
                }
                
                //Step 8: START COMMON SPECS
                var origin = _mCameraStatsFromDataString.values[i][8];
                var itemType = _mCameraStatsFromDataString.values[i][9];
                var quality = _mCameraStatsFromDataString.values[i][10];
                
                //Step 8.1: Check Origin
                var isOriginValid = Enum.TryParse<ItemOrigin>(origin, out var originValue);
                if (!isOriginValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have origin set");
                    return;
                }
                
                //Step 8.2: Check ItemType
                var itemTypes = itemType.Split(',');
                if(itemTypes.Any(x=> Enum.TryParse<ItemBaseType>(x, out var itemTypeValue) == false))
                {
                    Debug.LogWarning($"[LoadGuardDataFromJson] All Item types set for Guard {supplierBitId} must be valid");
                    return;
                }
                var castItemTypes = itemTypes.Select(x => Enum.Parse<ItemBaseType>(x)).ToList();

                //Step 8.3: Check Item Quality
                var isItemQualityValid = Enum.TryParse<ItemBaseQuality>(quality, out var itemQualityValue);
                if (!isItemQualityValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have item quality set");
                    return;
                }

                ICameraBaseData itemDataObject =
                    new CameraBaseData(supplierBitId,itemId,range, peopleInSight, clarity, persuasiveness, fovRadius, originValue, castItemTypes, itemQualityValue);
                _mCamerasSpecialData[supplierBitId].Add(itemDataObject);
            }
            _mGotCamerasData = true;
        }
        private void LoadWeaponDataFromJson(string sourceJson)
        {
            Debug.Log($"[ItemsDataController.LoadWeaponDataFromJson]");
            _mWeaponStatsFromDataString = JsonConvert.DeserializeObject<WeaponStatsFromData>(sourceJson);
            _mWeaponsSpecialData = new Dictionary<BitItemSupplier, List<IWeaponBaseData>>();
            if (_mWeaponStatsFromDataString == null)
            {
                Debug.LogError("[LoadWeaponDataFromJson] Weapon Stats data must be reachable");
                return;
            }
            var lastItemSupplierId = 1;
            var currentSupplierWeaponList = new List<IWeaponBaseData>();
            for (var i = 1; i < _mWeaponStatsFromDataString.values.Count; i++)
            {
                //Step 1: Get Item supplier Id
                var gotId = int.TryParse(_mWeaponStatsFromDataString.values[i][0], out var supplierId);
                if (!gotId)
                {
                    Debug.LogError("[LoadWeaponDataFromJson] Weapon Supplier Id must be available");
                    return;
                }
                var supplierBitId = (BitItemSupplier) supplierId;
                //Step 1.1: If its new supplier, add current list to Dict and clean.
                if (lastItemSupplierId != supplierId || i == 1)
                {
                    _mWeaponsSpecialData.Add(supplierBitId, currentSupplierWeaponList);
                    currentSupplierWeaponList = new List<IWeaponBaseData>();
                    lastItemSupplierId = supplierId;
                }
                //Step 2: Get Item ID
                var gotItemId = int.TryParse(_mWeaponStatsFromDataString.values[i][1], out var itemId);
                if (!gotItemId)
                {
                    Debug.LogError("[LoadWeaponDataFromJson] Weapon BitItemId must be available");
                    return;
                }
                
                //Step 3: Get Weapon Quality
                var gotWeaponQuality = int.TryParse(_mWeaponStatsFromDataString.values[i][3], out var weaponQuality);
                if (!gotWeaponQuality)
                {
                    Debug.LogWarning("[LoadWeaponDataFromJson] Weapon Type must be available");
                }
                //Step 4: Get Damage
                var gotDamage = int.TryParse(_mWeaponStatsFromDataString.values[i][4], out var damage);
                if (!gotDamage)
                {
                    Debug.LogWarning("[LoadWeaponDataFromJson] Weapon Damage must be available");
                }
                //Step 5: Get Range
                var gotRange = int.TryParse(_mWeaponStatsFromDataString.values[i][5], out var range);
                if (!gotRange)
                {
                    Debug.LogWarning("[LoadWeaponDataFromJson] Weapon Range must be available");
                }
                //Step 6: Get Persuasiveness
                var gotPersuasiveness = int.TryParse(_mWeaponStatsFromDataString.values[i][6], out var persuasiveness);
                if (!gotPersuasiveness)
                {
                    Debug.LogWarning("[LoadWeaponDataFromJson] Weapon Persuasiveness must be available");
                }
                
                var gotPrecision = int.TryParse(_mWeaponStatsFromDataString.values[i][7], out var precision);
                if (!gotPrecision)
                {
                    Debug.LogWarning("[LoadWeaponDataFromJson] Weapon Precision must be available");
                }
                
                //Step 8: START COMMON SPECS
                var origin = _mWeaponStatsFromDataString.values[i][8];
                var itemType = _mWeaponStatsFromDataString.values[i][9];
                var quality = _mWeaponStatsFromDataString.values[i][10];
                
                //Step 8.1: Check Origin
                var isOriginValid = Enum.TryParse<ItemOrigin>(origin, out var originValue);
                if (!isOriginValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have origin set");
                    return;
                }
                
                //Step 8.2: Check ItemType
                var itemTypes = itemType.Split(',');
                if(itemTypes.Any(x=> Enum.TryParse<ItemBaseType>(x, out var itemTypeValue) == false))
                {
                    Debug.LogWarning($"[LoadGuardDataFromJson] All Item types set for Guard {supplierBitId} must be valid");
                    return;
                }
                var castItemTypes = itemTypes.Select(x => Enum.Parse<ItemBaseType>(x)).ToList();

                
                //Step 8.3: Check Item Quality
                var isItemQualityValid = Enum.TryParse<ItemBaseQuality>(quality, out var itemQualityValue);
                if (!isItemQualityValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have item quality set");
                    return;
                }
                IWeaponBaseData itemDataObject =
                    new WeaponBaseData(supplierBitId, itemId,weaponQuality, damage, range, persuasiveness, precision, originValue, castItemTypes, itemQualityValue);
                _mWeaponsSpecialData[supplierBitId].Add(itemDataObject);
            }
            _mGotWeaponsData = true;
        }
        private void LoadTrapDataFromJson(string sourceJson)
        {
            Debug.Log($"[ItemsDataController.LoadTrapDataFromJson]");
            _mTrapStatsFromDataString = JsonConvert.DeserializeObject<TrapStatsFromData>(sourceJson);
            _mTrapsSpecialData = new Dictionary<BitItemSupplier, List<ITrapBaseData>>();
            
            var lastItemSupplierId = 1;
            var currentSupplierTrapList = new List<ITrapBaseData>();
            if (_mTrapStatsFromDataString == null)
            {
                Debug.LogError("[LoadTrapDataFromJson] Trap Stats data must be reachable");
                return;
            }
            for (var i = 1; i < _mTrapStatsFromDataString.values.Count; i++)
            {
                //Step 1: Get Item supplier Id
                var gotId = int.TryParse(_mTrapStatsFromDataString.values[i][0], out var supplierId);
                if (!gotId)
                {
                    Debug.LogError("[LoadTrapDataFromJson] Weapon Supplier Id must be available");
                    return;
                }
                var supplierBitId = (BitItemSupplier) supplierId;
                //Step 1.1: If its new supplier, add current list to Dict and clean.
                if (lastItemSupplierId != supplierId || i == 1)
                {
                    _mTrapsSpecialData.Add(supplierBitId, currentSupplierTrapList);
                    currentSupplierTrapList = new List<ITrapBaseData>();
                    lastItemSupplierId = supplierId;
                }
                //Step 2: Get Item ID
                var gotItemId = int.TryParse(_mTrapStatsFromDataString.values[i][1], out var itemId);
                if (!gotItemId)
                {
                    Debug.LogError("[LoadTrapDataFromJson] Weapon BitItemId must be available");
                    return;
                }
                
                //Step 3: Get Effectiveness
                var gotEffectiveness = int.TryParse(_mTrapStatsFromDataString.values[i][3], out var effectiveness);
                if (!gotEffectiveness)
                {
                    Debug.LogWarning("[LoadTrapDataFromJson] Effectiveness must be available");
                }
                //Step 4: Get Damage
                var gotDamage = int.TryParse(_mTrapStatsFromDataString.values[i][4], out var damage);
                if (!gotDamage)
                {
                    Debug.LogWarning("[LoadTrapDataFromJson] Weapon Damage must be available");
                }
                //Step 5: Get Range
                var gotRange = int.TryParse(_mTrapStatsFromDataString.values[i][5], out var range);
                if (!gotRange)
                {
                    Debug.LogWarning("[LoadTrapDataFromJson] Weapon Range must be available");
                }
                //Step 6: Get Persuasiveness
                var gotPersuasiveness = int.TryParse(_mTrapStatsFromDataString.values[i][6], out var persuasiveness);
                if (!gotPersuasiveness)
                {
                    Debug.LogWarning("[LoadTrapDataFromJson] Weapon Persuasiveness must be available");
                }
                //Step 8: START COMMON SPECS
                var origin = _mTrapStatsFromDataString.values[i][7];
                var itemType = _mTrapStatsFromDataString.values[i][8];
                var quality = _mTrapStatsFromDataString.values[i][9];
                
                //Step 8.1: Check Origin
                var isOriginValid = Enum.TryParse<ItemOrigin>(origin, out var originValue);
                if (!isOriginValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have origin set");
                    return;
                }
                
                //Step 8.2: Check ItemType
                var itemTypes = itemType.Split(',');
                if(itemTypes.Any(x=> Enum.TryParse<ItemBaseType>(x, out var itemTypeValue) == false))
                {
                    Debug.LogWarning($"[LoadGuardDataFromJson] All Item types set for Guard {supplierBitId} must be valid");
                    return;
                }
                var castItemTypes = itemTypes.Select(x => Enum.Parse<ItemBaseType>(x)).ToArray();
                var itemTypesFinal = BitOperator.TurnEnumsToInt(castItemTypes);
                
                //Step 8.3: Check Item Quality
                var isItemQualityValid = Enum.TryParse<ItemBaseQuality>(quality, out var itemQualityValue);
                if (!isItemQualityValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have item quality set");
                    return;
                }
                ITrapBaseData itemDataObject =
                    new TrapBaseData(supplierBitId, itemId, effectiveness, damage, range, persuasiveness,  originValue, itemTypesFinal, itemQualityValue);
                _mTrapsSpecialData[supplierBitId].Add(itemDataObject);
            }
            _mGotTrapsData = true;
        }
        private void LoadOtherItemsFromJson(string sourceJson)
        {
            Debug.Log($"[ItemsDataController.LoadOtherItemsFromJson]");
            _mOtherItemsStatsFromDataString = JsonConvert.DeserializeObject<OtherItemsStatsFromData>(sourceJson);
            _mOthersSpecialData = new Dictionary<BitItemSupplier, List<IOtherItemBaseData>>();
            
            var lastItemSupplierId = 1;
            var currentSupplierOtherItemsList = new List<IOtherItemBaseData>();
            if (_mOtherItemsStatsFromDataString == null)
            {
                Debug.LogError("[LoadOtherItemsFromJson] Other items Stats data must be reachable");
                return;
            }
            for (var i = 1; i < _mOtherItemsStatsFromDataString.values.Count; i++)
            {
                //Step 1: Get Item supplier Id
                var gotId = int.TryParse(_mOtherItemsStatsFromDataString.values[i][0], out var supplierId);
                if (!gotId)
                {
                    Debug.LogError("[LoadOtherItemsFromJson] Other Item Supplier Id must be available");
                    return;
                }
                var supplierBitId = (BitItemSupplier) supplierId;
                //Step 1.1: If its new supplier, add current list to Dict and clean.
                if (lastItemSupplierId != supplierId || i == 1)
                {
                    _mOthersSpecialData.Add(supplierBitId, currentSupplierOtherItemsList);
                    currentSupplierOtherItemsList = new List<IOtherItemBaseData>();
                    lastItemSupplierId = supplierId;
                }
                //Step 2: Get Item ID
                var gotItemId = int.TryParse(_mOtherItemsStatsFromDataString.values[i][1], out var itemId);
                if (!gotItemId)
                {
                    Debug.LogError("[LoadOtherItemsFromJson] Other Item BitItemId must be available");
                    return;
                }
                
                //Step 3: Get Effectiveness
                var gotEffectiveness = int.TryParse(_mOtherItemsStatsFromDataString.values[i][3], out var effectiveness);
                if (!gotEffectiveness)
                {
                    Debug.LogWarning("[LoadOtherItemsFromJson] Effectiveness must be available");
                }
                //Step 4: Get Damage
                var gotDamage = int.TryParse(_mOtherItemsStatsFromDataString.values[i][4], out var damage);
                if (!gotDamage)
                {
                    Debug.LogWarning("[LoadOtherItemsFromJson] Weapon Damage must be available");
                }
                //Step 5: Get Range
                var gotRange = int.TryParse(_mOtherItemsStatsFromDataString.values[i][5], out var range);
                if (!gotRange)
                {
                    Debug.LogWarning("[LoadOtherItemsFromJson] Weapon Range must be available");
                }
                //Step 6: Get Persuasiveness
                var gotPersuasiveness = int.TryParse(_mOtherItemsStatsFromDataString.values[i][6], out var persuasiveness);
                if (!gotPersuasiveness)
                {
                    Debug.LogWarning("[LoadOtherItemsFromJson] Weapon Persuasiveness must be available");
                }
                //Step 8: START COMMON SPECS
                var origin = _mOtherItemsStatsFromDataString.values[i][7];
                var itemType = _mOtherItemsStatsFromDataString.values[i][8];
                var quality = _mOtherItemsStatsFromDataString.values[i][9];
                
                //Step 8.1: Check Origin
                var isOriginValid = Enum.TryParse<ItemOrigin>(origin, out var originValue);
                if (!isOriginValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have origin set");
                    return;
                }
                
                //Step 8.2: Check ItemType
                var itemTypes = itemType.Split(',');
                if(itemTypes.Any(x=> Enum.TryParse<ItemBaseType>(x, out var itemTypeValue) == false))
                {
                    Debug.LogWarning($"[LoadGuardDataFromJson] All Item types set for Guard {supplierBitId} must be valid");
                    return;
                }
                var castItemTypes = itemTypes.Select(x => Enum.Parse<ItemBaseType>(x)).ToList();

                
                //Step 8.3: Check Item Quality
                var isItemQualityValid = Enum.TryParse<ItemBaseQuality>(quality, out var itemQualityValue);
                if (!isItemQualityValid)
                {
                    Debug.LogWarning("[LoadGuardDataFromJson] Guard must have item quality set");
                    return;
                }
                IOtherItemBaseData iOtherItemsDataObject =
                    new OtherItemBaseData(supplierBitId,itemId,effectiveness, damage, range, persuasiveness,  originValue, castItemTypes, itemQualityValue);
                _mOthersSpecialData[supplierBitId].Add(iOtherItemsDataObject);
            }
            _mGotOtherItemsData = true;
        }
        #endregion

        private void LoadBaseItemDataFromJson(string sourceJson)
        {
            Debug.Log($"[ItemsDataController.LoadBaseItemDataFromJson]");
            Debug.Log($"StartParsing Items Catalogue: {sourceJson}");

            _mItemCatalogueDataString = JsonConvert.DeserializeObject<ItemCatalogueFromData>(sourceJson);
            _mBaseCatalogueBaseItemsFromData = new Dictionary<BitItemSupplier, List<IItemObject>>();
            
            var lastItemSupplierId = 1;
            List<IItemObject> currentSupplierList = new List<IItemObject>();
            if (_mItemCatalogueDataString == null)
            {
                Debug.LogError($"[LoadBaseItemDataFromJson] _mItemCatalogueDataString must be reachable");
                return;
            }
            if (_mBaseCatalogueBaseItemsFromData == null)
            {
                Debug.LogError($"_mItemCatalogueDataString must not be null after parsing");
                return;
            }
            for (var i = 1; i < _mItemCatalogueDataString.values.Count; i++)
            {
                //Step 1: Get Item supplier Id
                var gotId = int.TryParse(_mItemCatalogueDataString.values[i][0], out var supplierId);
                if (!gotId)
                {
                    Debug.LogError("[LoadBaseItemDataFromJson] Item must have a supplier set from data");
                    return;
                }
                var supplierBitId = (BitItemSupplier) supplierId;
                //Step 1.1: If its new supplier, add current list to Dict and clean.
                if (lastItemSupplierId != supplierId || i == 1)
                {
                    _mBaseCatalogueBaseItemsFromData.Add(supplierBitId, currentSupplierList);
                    currentSupplierList = new List<IItemObject>();
                    lastItemSupplierId = supplierId;
                }
                
                //Step 2: Get Item Type
                var gotItemType = int.TryParse(_mItemCatalogueDataString.values[i][1], out var itemType);
                if (!gotItemType)
                {
                    Debug.LogError("[LoadBaseItemDataFromJson] Item must have an item type set from data");
                    return;
                }
                var bitItemType = (BitItemType) itemType;
                
                //Step 3: Get Item ID
                var gotItemId = int.TryParse(_mItemCatalogueDataString.values[i][2], out var itemId);
                if (!gotItemId)
                {
                    Debug.LogError("[LoadBaseItemDataFromJson] Item must have a BitID set from data");
                    return;
                }
                
                //Step 4: Get Item name
                var itemName = _mItemCatalogueDataString.values[i][3];
                
                //Step 5: Get Item Unlock Points
                var gotUp = int.TryParse(_mItemCatalogueDataString.values[i][4], out var unlockPoints);
                if (!gotUp)
                {
                    Debug.LogWarning("[LoadBaseItemDataFromJson] Item Unlock Points must be available in data");
                }

                //Step 6: Get Item price
                var gotPrice = int.TryParse(_mItemCatalogueDataString.values[i][5], out var itemPrice);
                if (!gotPrice)
                {
                    Debug.LogWarning("[LoadBaseItemDataFromJson] Item Price must be available in data");
                }
                //Step 7: Get Item Description
                var itemDescription = _mItemCatalogueDataString.values[i][6];
                //Step 8: Get Item Sprite Icon name
                var spriteName = _mItemCatalogueDataString.values[i][7];
                //Step 9: Get Item Actions 
                var gotMaxAmount = int.TryParse(_mItemCatalogueDataString.values[i][8], out var itemStoreAmount);
                if (!gotMaxAmount)
                {
                    Debug.LogWarning("[LoadBaseItemDataFromJson] Item Actions must be available in data");
                }
                //Step10: GetPrefabName
                var gamePrefabName = _mItemCatalogueDataString.values[i][9];

                //var itemStats data
                //IItemTypeStats itemSpecialStats = GetItemSpecialStats(supplierBitId, bitItemType, itemId);
                
                //Step 9: Create Item Scriptable Object and add to List in Dictionary
                IItemObject itemDataObject = ScriptableObject.CreateInstance<ItemObject>();
                itemDataObject.SetItemObjectData(supplierBitId, bitItemType, itemId, itemName, unlockPoints, 
                    itemPrice,itemDescription, spriteName, itemStoreAmount, gamePrefabName);
                _mBaseCatalogueBaseItemsFromData[supplierBitId].Add(itemDataObject);
                Debug.Log($"Added Item: {itemDataObject.ItemName}");
                Debug.Log($"Current Item Supplier: {itemDataObject.ItemSupplier}");
            }

            _mBaseDataIsReady = true;
        }

        public IItemTypeStats GetItemStats(BitItemSupplier supplier, BitItemType itemType, int itemId)
        {
            try
            {
                switch (itemType)
                {
                    case BitItemType.GUARD_ITEM_TYPE:
                        return GetStatsForGuard(supplier, itemId);
                    case BitItemType.CAMERA_ITEM_TYPE:
                        return GetStatsForCamera(supplier, itemId);
                    case BitItemType.WEAPON_ITEM_TYPE:
                        return GetStatsForWeapon(supplier, itemId);
                    case BitItemType.TRAP_ITEM_TYPE:
                        return GetStatsForTrap(supplier, itemId);
                    case BitItemType.SPECIAL_ITEM_TYPE:
                        return GetStatsForOtherItemsType(supplier, itemId);
                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private void InjectItemSpecialStats()
        {
            if(_mInjectedData)
                return;
            foreach (var itemSupplier in _mBaseCatalogueBaseItemsFromData)
            {
                foreach (var itemObject in itemSupplier.Value)
                {
                    var specialStats = GetItemStats(itemSupplier.Key, itemObject.ItemType ,itemObject.BitId);
                    itemObject.SetItemSpecialStats(specialStats);
                }
            }
            _mInjectedData = true;
        }
        #endregion


        public IGuardBaseData GetStatsForGuard(BitItemSupplier itemSupplier, int itemBitId)
        {
            Debug.Log("[GetStatsForGuard]");
            try
            {
                return _mGuardsSpecialData[itemSupplier].SingleOrDefault(x => x.Id == itemBitId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public ICameraBaseData GetStatsForCamera(BitItemSupplier itemSupplier, int itemBitId)
        {
            try
            {
                Debug.Log($"[GetStatsForCamera] SupplierID: {itemSupplier}. ItemId: {itemBitId}");
                return _mCamerasSpecialData[itemSupplier].SingleOrDefault(x => x.Id == itemBitId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IWeaponBaseData GetStatsForWeapon(BitItemSupplier itemSupplier, int itemBitId)
        {
            try
            {
                Debug.Log("[GetStatsForWeapon]");
                return _mWeaponsSpecialData[itemSupplier].SingleOrDefault(x => x.Id == itemBitId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public ITrapBaseData GetStatsForTrap(BitItemSupplier itemSupplier, int itemBitId)
        {
            try
            {
                Debug.Log("[GetStatsForTrap]");
                return _mTrapsSpecialData[itemSupplier].SingleOrDefault(x => x.Id == itemBitId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public IOtherItemBaseData GetStatsForOtherItemsType(BitItemSupplier itemSupplier, int itemBitId)
        {
            try
            {
                Debug.Log("[GetStatsForOtherItemsType]");
                return _mOthersSpecialData[itemSupplier].SingleOrDefault(x => x.Id == itemBitId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}