using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DataUnits.ItemSources;
using DialogueSystem;
using GameDirection;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.Players_NPC;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace DataUnits.GameRequests
{
    public class Omnicorp : IOmnicorp
    {
        private IRequestModule _mRequestModule;
        public Omnicorp()
        {
            _mRequestModule = new RequestModule(DialogueSpeakerId.Omnicorp);
        }
    }

    public interface IOmnicorp
    {
    }

    public interface IRequestsModuleManager : IProfileModule
    {
        public void HandleIncomingRequest(DialogueSpeakerId speaker, int requestId);
    }
    public class RequestsModuleManager : IRequestsModuleManager
    {
        //Base all catalogue data
        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mBaseRequestsData = new Dictionary<DialogueSpeakerId, List<IGameRequest>>();
        private RequestCatalogueData _mRequestsCatalogue;

        
        private IJobSourcesModuleData _mJobsSourcesModule;
        private IItemSuppliersModuleData _mItemSuppliersModule;
        
        
        private Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests { get; set; }
        
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mJobsSourcesModule = currentPlayerProfile.GetActiveJobsModule();
            _mItemSuppliersModule = currentPlayerProfile.GetActiveItemSuppliersModule();
            var url = DataSheetUrls.BaseGameRequests;
            GameDirector.Instance.ActCoroutine(LoadRequestsBaseData(url));
        }

        public void HandleIncomingRequest(DialogueSpeakerId speaker, int requestId)
        {
            //If speaker doesn't exist in Request data. Return
            if (!_mBaseRequestsData.ContainsKey(speaker))
            {
                Debug.LogWarning("Base Request Data Must contain the Request Event Id Keys");
                return;
            }
            if(!_mBaseRequestsData.Any(x=>x.Value.Any(r=>r.RequestId == requestId)))
            {
                Debug.LogWarning("Request Id must be available in the Request Data");
                return;
            }
            var request = _mBaseRequestsData[speaker].Find(r => r.RequestId == requestId);
               
        }

        private IEnumerator LoadRequestsBaseData(string url)
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
                LoadRequestsCatalogue(sourceJson);
            }
        }
        private void LoadRequestsCatalogue(string sourceJson)
        {
            Debug.Log($"RequestsModuleManager.LoadRequestsCatalogue");
            _mRequestsCatalogue = JsonConvert.DeserializeObject<RequestCatalogueData>(sourceJson);
            
            Debug.Log($"Finished parsing. Is _mRentValuesFromDataString null?: {_mRequestsCatalogue == null}");

            int currentSpeaker = 0;
            int requestCount = 0;
            for (var i = 1; i < _mRequestsCatalogue!.values.Count;i++)
            {
                var hasSpeakerId = int.TryParse(_mRequestsCatalogue.values[i][0], out var speakerId);
                if(speakerId != currentSpeaker || i == 1)
                {
                    currentSpeaker = speakerId;
                    _mBaseRequestsData.Add((DialogueSpeakerId)currentSpeaker, new List<IGameRequest>());
                }

                requestCount++;
                var hasRequestId = int.TryParse(_mRequestsCatalogue.values[i][1], out var requestId);
                var requestTitle = _mRequestsCatalogue.values[i][2];
                var requestDescription = _mRequestsCatalogue.values[i][3];
                
                var hasRequestType = Enum.TryParse(_mRequestsCatalogue.values[i][4], out RequirementActionType requestType);
                var hasReqObject = Enum.TryParse(_mRequestsCatalogue.values[i][5], out RequirementObjectType requestObject);
                var hasReqLogic = Enum.TryParse(_mRequestsCatalogue.values[i][6], out RequirementLogicEvaluator requestLogic);
                var hasReqParameterType = Enum.TryParse(_mRequestsCatalogue.values[i][7], out RequirementConsideredParameter requestParameter);
                
                if(!hasRequestType || !hasReqObject || !hasReqLogic || !hasReqParameterType)
                {
                    Debug.LogError("[LoadRentDataFromJson] Request Must have all parameters available");
                    return;
                }

                var requirementValues = _mRequestsCatalogue.values[i][8].Split(',');
                var hasQuantity = int.TryParse(_mRequestsCatalogue.values[i][9], out var quantity);
                
                var finalRequest = Factory.CreateGameRequest(speakerId, requestId, requestTitle, requestDescription, requestType, 
                    requestObject, requestLogic, requestParameter, requirementValues, quantity);
                _mBaseRequestsData[(DialogueSpeakerId)speakerId].Add(finalRequest);
            }
            Debug.Log($"[LoadRentDataFromJson]Finished parsing process for LoadRequestsCatalogue. Number of Requests is {requestCount}");
        }
    }
    
    public class RequestCatalogueData : CatalogueFromDataGeneric
    {
    }
}