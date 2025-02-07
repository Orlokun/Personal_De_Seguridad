using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using GamePlayManagement;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using UnityEngine;
using Utils;

namespace DataUnits.GameRequests
{
    public interface IRequestsModuleManager : IProfileModule
    {
        public void HandleIncomingRequestActivation(DialogueSpeakerId speaker, int requestId);
    }
    
    public class RequestsModuleManager : IRequestsModuleManager
    {
        private IRequestModuleManagerData _mRequestModuleData;
        
        private IJobSourcesModuleData _mJobsSourcesModule;
        private IItemSuppliersModuleData _mItemSuppliersModule;

        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mActiveRequests = new();
        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mAchievedRequests = new();
        private Dictionary<DialogueSpeakerId, List<IGameRequest>> _mFailedRequests = new();

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