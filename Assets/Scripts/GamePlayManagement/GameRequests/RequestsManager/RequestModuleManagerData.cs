using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues.JsonCatalogueLoaders;
using DialogueSystem.Units;
using GameDirection;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.GameRequests.RewardsPenalties;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Utils;

namespace GamePlayManagement.GameRequests.RequestsManager
{
    public class RequestModuleManagerData : IRequestModuleManagerData
    {
        private RequestCatalogueData _mRequestsCatalogue;
        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mBaseRequestsData = new Dictionary<DialogueSpeakerId, List<IGameRequest>>();
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> BaseRequestsData => _mBaseRequestsData;

        public bool RequestExistsInData(DialogueSpeakerId requesterSpeakerId, int requestId)
        {
            return _mBaseRequestsData.ContainsKey(requesterSpeakerId) && _mBaseRequestsData[requesterSpeakerId].Any(x=>x.RequestId == requestId);
        }

        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mActiveRequests = new();
        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mCompletedRequests = new();
        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mFailedRequests = new();

        public void AddCompletedRequestInData(DialogueSpeakerId requester, IGameRequest request)
        {
            if(CompletedRequests == null)
            {
                _mCompletedRequests = new Dictionary<DialogueSpeakerId, List<IGameRequest>>();
            }
            if (!_mCompletedRequests.TryGetValue(requester, out var completedRequest))
            {
                _mCompletedRequests.Add(requester, new List<IGameRequest>());
                _mCompletedRequests[requester].Add(request);
                return;
            }
            if(completedRequest.Any(x=>x.RequestId == request.RequestId))
            {
                Debug.LogWarning($"Request for {requester} with Id {request.RequestId} is already completed.");
                return;
            }
            _mCompletedRequests[requester].Add(request);
        }
        
        public void AddFailedRequestInData(DialogueSpeakerId requester, IGameRequest request)
        {
            if(FailedRequests == null)
            {
                _mFailedRequests = new Dictionary<DialogueSpeakerId, List<IGameRequest>>();
            }
            if (!_mFailedRequests.TryGetValue(requester, out var failedRequests))
            {
                _mFailedRequests.Add(requester, new List<IGameRequest>());
                _mFailedRequests[requester].Add(request);
                return;
            }
            if(failedRequests.Any(x=>x.RequestId == request.RequestId))
            {
                Debug.LogWarning($"Request for {requester} with Id {request.RequestId} is already failed.");
                return;
            }
            _mFailedRequests[requester].Add(request);
        }

        public void ResolveFinishedRequest(DialogueSpeakerId requester, IGameRequest request)
        {
            if (request.RequestStatus == RequestStatus.Completed)
            {
                AddCompletedRequestInData(requester, request);
            }
            else if (request.RequestStatus == RequestStatus.Failed)
            {
                AddFailedRequestInData(requester, request);
            }
        }

        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests => _mActiveRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests => _mCompletedRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests => _mFailedRequests;
        
        public void ActivateRequestInData(IGameRequest request)
        {
            if (_mActiveRequests == null)
            {
                _mActiveRequests = new Dictionary<DialogueSpeakerId, List<IGameRequest>>();
            }
            if (!_mActiveRequests.ContainsKey(request.RequesterSpeakerId))
            {
                _mActiveRequests.Add(request.RequesterSpeakerId, new List<IGameRequest>());
            }
            if(_mActiveRequests[request.RequesterSpeakerId].Any(r => r.RequestId == request.RequestId))
            {
                Debug.LogWarning($"Request for {request.RequesterSpeakerId} with Id {request.RequestId} is already active.");
                return;
            }
            _mActiveRequests[request.RequesterSpeakerId].Add(request);
            Debug.Log("[ActivateRequestInData] Request is activated!");
        }
        
        
        #region DataWebRequest
        public void LoadRequestsData(string url)
        {
            GameDirector.Instance.ActCoroutine(LoadRequestsBaseData(url));
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
                
                var rewardValues = _mRequestsCatalogue.values[i][10].Split('|');
                var punishmentValues = _mRequestsCatalogue.values[i][11].Split('|');
                
                var hasTargetDay = Enum.TryParse(_mRequestsCatalogue.values[i][12], out DayBitId targetDayId);
                var hasTargetHour = Enum.TryParse(_mRequestsCatalogue.values[i][13], out PartOfDay targetHourId);
                
                if(!hasTargetDay || !hasTargetHour)
                {
                    Debug.LogError("[LoadRentDataFromJson] Request Must have Day Target values available");
                    return;
                }
                
                var finalRequest = Factory.CreateGameRequest(speakerId, requestId, requestTitle, requestDescription, requestType, 
                    requestObject, requestLogic, requestParameter, requirementValues, quantity, rewardValues, punishmentValues, targetDayId, targetHourId);
                _mBaseRequestsData[(DialogueSpeakerId)speakerId].Add(finalRequest);
            }
            Debug.Log($"[LoadRentDataFromJson]Finished parsing process for LoadRequestsCatalogue. Number of Requests is {requestCount}");
        }
        #endregion

    }
}