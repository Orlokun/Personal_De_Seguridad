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
    public class TransportValuesCatalogue : MonoBehaviour, ITransportValuesCatalogue
    {
        private static TransportValuesCatalogue _instance;
        public static ITransportValuesCatalogue Instance => _instance;
        private TransportValuesFromData _mTransportValuesFromDataString;

        private List<ITransportDataObject> _mTransportDataObjects;
        public List<ITransportDataObject> GetAllTransportDataObjects => _mTransportDataObjects;

        public ITransportDataObject GetTransportDataObject(TransportTypesId id) =>
            _mTransportDataObjects.SingleOrDefault(x => x.TransportId == id);

        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (_instance != null)
            {
                Destroy(this);
            }

            _instance = this;
            GetTransportCatalogueData();
        }

        private void GetTransportCatalogueData()
        {
            Debug.Log($"START: Collecting Transport Objects DATA");
            var url = DataSheetUrls.TransportValuesData;
            StartCoroutine(LoadTransport(url));
        }

        private IEnumerator LoadTransport(string url)
        {
            var webRequest = UnityWebRequest.Get(url);
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Get Transport Catalogue Data must be reachable");
            }
            else
            {
                var sourceJson = webRequest.downloadHandler.text;
                LoadTransportDataFromJson(sourceJson);
            }
        }
        private void LoadTransportDataFromJson(string sourceJson)
        {
            Debug.Log($"TransportValuesCatalogue.LoadTransportDataFromJson");
            Debug.Log($"StartParsing Transport Data Catalogue: {sourceJson}");

            _mTransportValuesFromDataString = JsonConvert.DeserializeObject<TransportValuesFromData>(sourceJson);
            Debug.Log($"Finished parsing. Is _mRentValuesFromDataString null?: {_mTransportValuesFromDataString == null}");
            _mTransportDataObjects = new List<ITransportDataObject>();
            
            for (var i = 1; i < _mTransportValuesFromDataString!.values.Count;i++)
            {

                var gotId = int.TryParse(_mTransportValuesFromDataString.values[i][0], out var transportId);
                
                var transportName = _mTransportValuesFromDataString.values[i][1];
                
                var gotPrice = int.TryParse(_mTransportValuesFromDataString.values[i][2], out var transportPrice);

                var gotUnlockPoints = int.TryParse(_mTransportValuesFromDataString.values[i][3], out var unlockPoints);

                var gotSpecialCondition = int.TryParse(_mTransportValuesFromDataString.values[i][4], out var specialCondition);
                var gotTransportEnergy = int.TryParse(_mTransportValuesFromDataString.values[i][6], out var energy);
                var gotTransportSanity = int.TryParse(_mTransportValuesFromDataString.values[i][7], out var sanity);
                if (!gotId || transportName.Equals(default) || !gotPrice || !gotUnlockPoints || !gotSpecialCondition ||
                    !gotTransportEnergy || !gotTransportSanity)
                {
                    Debug.LogError("[LoadTransportDataFromJson] All values in Transport objects data must be filled!");
                }

                var transportDataObject = Factory.CreateTransportDataObject((TransportTypesId)transportId, transportName, transportPrice, 
                    unlockPoints, specialCondition, energy, sanity);
                _mTransportDataObjects.Add(transportDataObject);
            }
            Debug.Log($"[LoadRentDataFromJson]Finished parsing process for RentValuesCatalogue");
        }
    }
}