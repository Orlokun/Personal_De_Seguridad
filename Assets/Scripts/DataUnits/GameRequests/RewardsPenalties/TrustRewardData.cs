using DialogueSystem.Units;

namespace DataUnits.GameRequests.RewardsPenalties
{
    public class TrustRewardData : RewardData, ITrustRewardData
    {
        private int _mRewardValue;
        private readonly DialogueSpeakerId _mTrustGiverSpeakerId;

        public TrustRewardData(RewardTypes rewardType, string[] rewardRawValues, DialogueSpeakerId trustGiverSpeakerId) : base(rewardType, rewardRawValues)
        {
            _mTrustGiverSpeakerId = trustGiverSpeakerId;
            _mRewardValue = int.Parse(rewardRawValues[1]);
        }
        public int TrustAmount => _mRewardValue;
        public DialogueSpeakerId TrustGiverSpeakerId => _mTrustGiverSpeakerId;
    }
}