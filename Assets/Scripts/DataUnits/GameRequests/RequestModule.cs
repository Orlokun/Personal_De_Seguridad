using System;
using System.Collections.Generic;
using DialogueSystem;
using GamePlayManagement.Players_NPC;
using Utils;

namespace DataUnits.GameRequests
{
    public interface IRequestModule
    {
        public bool IsRequestActive(IGameRequest request);
        void ActivateRequest(IGameRequest request);
        public void EvaluateEvent(EventArgs args);
    }
    
    public class RequestModule : IRequestModule
    {
        private IRequestModuleData _mRequestModuleData;
        private DialogueSpeakerId _mSpeakerId;
        public RequestModule(DialogueSpeakerId speakerId)
        {
            _mSpeakerId = speakerId;
            _mRequestModuleData = Factory.CreateRequestModuleData(); 
        }

        public bool IsRequestActive(IGameRequest request)
        {
            return false;
        }
        
        public void ActivateRequest(IGameRequest request)
        {
            throw new System.NotImplementedException();
        }

        public void EvaluateEvent(EventArgs args)
        {
            
        }
    }

    internal class RequestModuleData : IRequestModuleData
    {
        private int _mActiveRequests;
        private Dictionary<int, IGameRequest> _mActiveRequestsData = new();

        private int _mCompletedRequests;
        private Dictionary<int, IGameRequest> _mCompletedRequestsData = new();
        
        private int _mFailedRequests;
        private Dictionary<int, IGameRequest>_mFailedRequestsData = new();
        
    }

    public interface IRequestModuleData
    {
    }
}