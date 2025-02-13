using DialogueSystem;
using GamePlayManagement.BitDescriptions.RequestParameters;

namespace DataUnits.GameRequests
{
    public interface IGameRequest
    {
        public DialogueSpeakerId RequesterSpeakerId { get; }
        public int RequestId { get; }
        public string ReqTitle { get; }
        public string ReqDescription { get; }
        public bool IsCompleted { get; }
        public void MarkAsCompleted();
        public RequirementActionType ChallengeActionType { get; }
    }
}