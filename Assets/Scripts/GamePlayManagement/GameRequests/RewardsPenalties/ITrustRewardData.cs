using DialogueSystem.Units;

namespace GamePlayManagement.GameRequests.RewardsPenalties
{
    public interface ITrustRewardData
    {
        int TrustAmount { get;}
        DialogueSpeakerId TrustGiverSpeakerId { get; }
    }
}