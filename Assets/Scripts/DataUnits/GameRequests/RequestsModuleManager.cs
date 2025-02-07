using System.Collections.Generic;
using DialogueSystem;
using GamePlayManagement;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using Utils;

namespace DataUnits.GameRequests
{
    public interface IRequestsModuleManager : IProfileModule
    {
        public void HandleIncomingRequestActivation(DialogueSpeakerId speaker, int requestId);
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests { get; }
    }
    
    public class RequestsModuleManager : IRequestsModuleManager
    {
        private IRequestModuleManagerData _mRequestModuleData;
        
        private IJobSourcesModuleData _mJobsSourcesModule;
        private IItemSuppliersModuleData _mItemSuppliersModule;

        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests => _mRequestModuleData.ActiveRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests => _mRequestModuleData.CompletedRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests => _mRequestModuleData.FailedRequests;

        #region Init
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mJobsSourcesModule = currentPlayerProfile.GetActiveJobsModule();
            _mItemSuppliersModule = currentPlayerProfile.GetActiveItemSuppliersModule();
            _mRequestModuleData = new RequestModuleManagerData();
            var url = DataSheetUrls.BaseGameRequests;
            _mRequestModuleData.LoadRequestsData(url);
        }
        #endregion

        public void HandleIncomingRequestActivation(DialogueSpeakerId speaker, int requestId)
        {
            //Confirm request exists in base data.
            if (!_mRequestModuleData.RequestExistsInData(speaker, requestId))
            {
                return;
            }
            var request = _mRequestModuleData.BaseRequestsData[speaker].Find(r => r.RequestId == requestId);
            _mRequestModuleData.ActivateRequestInData(request);
        }
    }
}