namespace DataUnits.GameRequests.RewardsPenalties
{
    public class OmniCreditRewardData : RewardData, IOmniCreditRewardData
    {
        private int _mRewardValue;
        public int OmniCreditsAmount => _mRewardValue;
        public OmniCreditRewardData(RewardTypes rewardType, string[] rewardRawValues) : base(rewardType, rewardRawValues)
        {
            _mRewardValue = int.Parse(rewardRawValues[1]);
        }
    }
}