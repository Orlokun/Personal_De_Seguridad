using DialogueSystem.Units;

namespace DataUnits.GameRequests.RewardsPenalties
{
    public interface ITrustRewardData
    {
        int TrustAmount { get;}
        DialogueSpeakerId TrustGiverSpeakerId { get; }
    }
}