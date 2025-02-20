using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using DialogueSystem.Units;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.RequestParameters;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules;
using GamePlayManagement.ProfileDataModules.ItemSuppliers;
using NUnit.Framework;
using UnityEngine;
using Utils;

namespace DataUnits.GameRequests
{
    public interface IRequestsModuleManager : IProfileModule
    {
        public void HandleIncomingRequestActivation(DialogueSpeakerId speaker, int requestId);
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests { get; }
        void CheckHireChallenges(JobSupplierBitId newJobSupplier);
    }
    
    public class RequestsModuleManager : IRequestsModuleManager
    {
        private IRequestModuleManagerData _mRequestModuleData;
        
        private IJobSourcesModuleData _mJobsSourcesModule;
        private IItemSuppliersModuleData _mItemSuppliersModule;

        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests => _mRequestModuleData.ActiveRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests => _mRequestModuleData.CompletedRequests;
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests => _mRequestModuleData.FailedRequests;

        //Completed Hire Challenge assume only one challenge is completed in the event. 
        public void CheckHireChallenges(JobSupplierBitId newJobSupplier)
        {
            if (ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }
            var completedHireChallenges = new List<int[]>();
            for (var supplierIndex = 0; supplierIndex<ActiveRequests.Count;supplierIndex++) 
            {
                var supplierRequests = ActiveRequests.ElementAt(supplierIndex);
                if (supplierRequests.Value.All(x => x.ChallengeActionType != RequirementActionType.Hire))
                {
                    continue;
                }
                var hireChallenges = supplierRequests.Value.Where(x => x.ChallengeActionType == RequirementActionType.Hire);
                foreach (var challenge in hireChallenges)
                {
                    var hireChallenge = (IHireGameRequest)challenge;
                    if (hireChallenge.JobHireObjective == newJobSupplier)
                    {
                        challenge.MarkAsCompleted();
                        completedHireChallenges.Add(new[] {supplierIndex, challenge.RequestId});
                        _mRequestModuleData.AddCompletedRequestInData((DialogueSpeakerId)supplierIndex, challenge);
                    }
                }
            }
            RemoveCompletedChallengesFromActive(completedHireChallenges);
        }

        private void RemoveCompletedChallengesFromActive(List<int[]> completedHireChallenges)
        {
            if(ActiveRequests == null || ActiveRequests.Count == 0)
            {
                return;
            }

            for (var i = 0; i<completedHireChallenges.Count;i++)
            {
                if (!ActiveRequests.ContainsKey((DialogueSpeakerId)completedHireChallenges[i][0]))
                {
                    continue;
                }
                var supplierRequests = ActiveRequests[(DialogueSpeakerId)completedHireChallenges[i][0]];
                if (supplierRequests.All(x => x.RequestId != completedHireChallenges[i][1]))
                {
                    return;
                }
                supplierRequests.Remove(supplierRequests.Find(x => x.RequestId == completedHireChallenges[i][1]));
            }
            Debug.Log("Cleared Completed Active Challenges");
        }

        #region Init
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _mJobsSourcesModule = currentPlayerProfile.GetActiveJobsModule();
            _mItemSuppliersModule = currentPlayerProfile.GetActiveItemSuppliersModule();
            _mRequestModuleData = new RequestModuleManagerData();
            var url = DataSheetUrls.BaseGameRequests;
            _mRequestModuleData.LoadRequestsData(url);
        }

        public void PlayerLostResetData()
        {
            ActiveRequests.Clear();
            CompletedRequests.Clear();
            FailedRequests.Clear();
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