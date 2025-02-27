﻿using System.Collections.Generic;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.ProfileDataModules;

namespace GamePlayManagement.GameRequests.RequestsManager
{
    public interface IRequestsModuleManager : IProfileModule
    {
        public void HandleIncomingRequestActivation(DialogueSpeakerId speaker, int requestId);
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> ActiveRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> CompletedRequests { get; }
        public Dictionary<DialogueSpeakerId, List<IGameRequest>> FailedRequests { get; }
        void CheckHireChallenges(JobSupplierBitId newJobSupplier);
    }
}