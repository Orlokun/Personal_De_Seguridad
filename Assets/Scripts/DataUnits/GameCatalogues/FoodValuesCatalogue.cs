using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.ItemSources;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.GameCatalogues
{
    public class FoodValuesCatalogue : MonoBehaviour, IFoodValuesCatalogue
    {
        private static FoodValuesCatalogue _instance;
        public static IFoodValuesCatalogue Instance => _instance;
        private FoodValuesFromData _mFoodValuesFromDataString;

        private List<IFoodDataObject> _mFoodDataObjects;
        public List<IFoodDataObject> GetAllFoodDataObjects => _mFoodDataObjects;
        public IFoodDataObject GetFoodDataObject(FoodTypesId id)=> _mFoodDataObjects.SingleOrDefault(x => x.FoodId == id);
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null)
            {
                Destroy(this);
            }
            _instance = this;
            GetFoodCatalogueData();
        }
        private void GetFoodCatalogueData()
        {
            Debug.Log($"START: Collecting Food Objects DATA");
            var url = DataSheetUrls.FoodValuesData;
            StartCoroutine(LoadRentCatalogueData(url));
        }
        private IEnumerator LoadRentCatalogueData(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Get Rent Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadFoodDataFromJson(sourceJson);
            }
        }
        private void LoadFoodDataFromJson(string sourceJson)
        {
            Debug.Log($"FoodValuesCatalogue.LoadFoodDataFromJson");
            Debug.Log($"StartParsing Food Data Catalogue: {sourceJson}");

            _mFoodValuesFromDataString = JsonConvert.DeserializeObject<FoodValuesFromData>(sourceJson);
            Debug.Log($"Finished parsing. Is _mRentValuesFromDataString null?: {_mFoodValuesFromDataString == null}");
            _mFoodDataObjects = new List<IFoodDataObject>();
            
            for (var i = 1; i < _mFoodValuesFromDataString!.values.Count;i++)
            {

                var gotId = int.TryParse(_mFoodValuesFromDataString.values[i][0], out var foodId);
                
                var foodName = _mFoodValuesFromDataString.values[i][1];
                
                var gotPrice = int.TryParse(_mFoodValuesFromDataString.values[i][2], out var foodPrice);

                var gotUnlockPoints = int.TryParse(_mFoodValuesFromDataString.values[i][3], out var unlockPoints);

                var gotSpecialCondition = int.TryParse(_mFoodValuesFromDataString.values[i][4], out var specialCondition);
                var gotFoodEnergy = int.TryParse(_mFoodValuesFromDataString.values[i][6], out var energy);
                var gotFoodSanity = int.TryParse(_mFoodValuesFromDataString.values[i][7], out var sanity);
                if (!gotId || foodName.Equals(default) || !gotPrice || !gotUnlockPoints || !gotSpecialCondition ||
                    !gotFoodEnergy || !gotFoodSanity)
                {
                    Debug.LogError("[LoadFoodDataFromJson] All values in Food objects data must be filled!");
                }

                var foodDataObject = Factory.CreateFoodDataObject((FoodTypesId)foodId, foodName, foodPrice, 
                    unlockPoints, specialCondition, energy, sanity);
                _mFoodDataObjects.Add(foodDataObject);
            }
            Debug.Log($"[LoadRentDataFromJson]Finished parsing process for RentValuesCatalogue");
        }

    }
}