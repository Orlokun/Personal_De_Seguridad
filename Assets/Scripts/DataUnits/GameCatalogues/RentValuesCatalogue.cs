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
    public class RentValuesCatalogue : MonoBehaviour, IRentValuesCatalogue
    {
        private static RentValuesCatalogue _instance;
        public static IRentValuesCatalogue Instance => _instance;
        
        private RentValuesFromData _mRentValuesFromDataString;
        private List<IRentDataObject> _mRentValuesList;
        public List<IRentDataObject> GetAllRentDataObjects => _mRentValuesList;
        public IRentDataObject GetRentObject(RentTypesId id) => _mRentValuesList.SingleOrDefault(x => x.RentId == id);
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null)
            {
                Destroy(this);
            }
            _instance = this;
            GetRentCatalogueData();
        }
        private void GetRentCatalogueData()
        {
            Debug.Log($"START: Collecting RENT Objects DATA");
            var url = DataSheetUrls.RentValuesData;
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
                LoadRentDataFromJson(sourceJson);
            }
        }
        
        private void LoadRentDataFromJson(string sourceJson)
        {
            Debug.Log($"RentValuesCatalogue.LoadRentDataFromJson");
            Debug.Log($"StartParsing Rent Data Catalogue: {sourceJson}");

            _mRentValuesFromDataString = JsonConvert.DeserializeObject<RentValuesFromData>(sourceJson);
            Debug.Log($"Finished parsing. Is _mRentValuesFromDataString null?: {_mRentValuesFromDataString == null}");
            _mRentValuesList = new List<IRentDataObject>();
            
            for (var i = 1; i < _mRentValuesFromDataString!.values.Count;i++)
            {

                var gotId = int.TryParse(_mRentValuesFromDataString.values[i][0], out var rentId);
                
                var rentName = _mRentValuesFromDataString.values[i][1];
                
                var gotPrice = int.TryParse(_mRentValuesFromDataString.values[i][2], out var rentPrice);

                var gotUnlockPoints = int.TryParse(_mRentValuesFromDataString.values[i][3], out var unlockPoints);

                var gotSpecialCondition = int.TryParse(_mRentValuesFromDataString.values[i][4], out var specialCondition);
                var gotRentEnergy = int.TryParse(_mRentValuesFromDataString.values[i][6], out var energy);
                var gotRentSanity = int.TryParse(_mRentValuesFromDataString.values[i][7], out var sanity);
                if (!gotId || rentName.Equals(default) || !gotPrice || !gotUnlockPoints || !gotSpecialCondition ||
                    !gotRentEnergy || !gotRentSanity)
                {
                    Debug.LogError("[LoadRentDataFromJson] All values in Rent objects data must be filled!");
                }

                var rentDataObject = Factory.CreateRentDataObject((RentTypesId)rentId, rentName, rentPrice, 
                    unlockPoints, specialCondition, energy, sanity);
                _mRentValuesList.Add(rentDataObject);
            }
            Debug.Log($"[LoadRentDataFromJson]Finished parsing process for RentValuesCatalogue");
        }
    }
}