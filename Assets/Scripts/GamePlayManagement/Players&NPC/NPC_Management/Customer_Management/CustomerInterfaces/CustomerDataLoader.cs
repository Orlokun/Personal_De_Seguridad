using System.Collections.Generic;
using System.Threading.Tasks;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem;
using GamePlayManagement.BitDescriptions.Suppliers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public class CustomerDataLoader : ICustomerDataLoader
{
    private Dictionary<JobSupplierBitId, ICustomersInstantiationFlowData> _mClientManagementData;

    public async Task LoadCustomerManagementDataAsync(string url)
    {
        var webRequest = UnityWebRequest.Get(url);
        await webRequest.SendWebRequest();
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"Jobs Catalogue management data must be reachable. Error: {webRequest.result}. {webRequest.error}");
        }
        else
        {
            var sourceJson = webRequest.downloadHandler.text;
            LoadCustomerDataFromJson(sourceJson);
        }
    }

    private void LoadCustomerDataFromJson(string sourceJson)
    {
        _mClientManagementData = new Dictionary<JobSupplierBitId, ICustomersInstantiationFlowData>();
        var customerManagementRawData = JsonConvert.DeserializeObject<StoreCustomerManagementData>(sourceJson);
        if (customerManagementRawData == null)
        {
            return;
        }
        for (var i = 1; i < customerManagementRawData.values.Count; i++)
        {
            var gotId = int.TryParse(customerManagementRawData.values[i][0], out var supplierId);
            var supplierBitId = (JobSupplierBitId)supplierId;
            if (supplierBitId == 0 || !gotId)
            {
                Debug.LogWarning($"[CustomerInSceneManager.LoadCustomerDataFromJson] Job Supplier must have Id greater than zero");
                return;
            }
            var gotMaxClients = int.TryParse(customerManagementRawData.values[i][2], out var maxClients);
            if (maxClients == 0 || !gotMaxClients)
            {
                Debug.LogWarning($"[CustomerInSceneManager.LoadProductsFromJson] Store must have max number of clients");
                return;
            }
            var storePrefabsPath = customerManagementRawData.values[i][3];
            var gotNumberOfPrefabs = int.TryParse(customerManagementRawData.values[i][4], out var maxPrefabs);
            if (maxPrefabs == 0 || !gotNumberOfPrefabs)
            {
                Debug.LogWarning($"[CustomerInSceneManager.LoadProductsFromJson]");
                return;
            }
            var instantiationRange = customerManagementRawData.values[i][5].Split(',');
            var castedRange = RangeProcessor.ProcessLinksStrings(instantiationRange);
            var gameDifficulty = 1;
            var customerManagementData = Factory.CreateCustomersInSceneManagerData(supplierBitId, gameDifficulty, maxClients, storePrefabsPath, castedRange);
            _mClientManagementData.Add(supplierBitId, customerManagementData);
        }
    }

    public Dictionary<JobSupplierBitId, ICustomersInstantiationFlowData> GetCustomerManagementData()
    {
        return _mClientManagementData;
    }
}
}