using System.Collections.Generic;
using DialogueSystem.Units;

namespace GamePlayManagement.GameRequests.RequestsManager
{
    internal interface IRequestModuleManagerData
    {
        void LoadRequestsData(string url);
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> BaseRequestsData { get; }

        public bool RequestExistsInData(DialogueSpeakerId requesterSpeakerId, int requestId);
        public void ActivateRequestInData(IGameRequest request);
        public void AddCompletedRequestInData(DialogueSpeakerId requester, IGameRequest request);
        public void AddFailedRequestInData(DialogueSpeakerId requester, IGameRequest request);
        public void ResolveFinishedRequest(DialogueSpeakerId requester, IGameRequest request);
        
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests { get; }
        
        public void ClearRequestModule();
    }
}