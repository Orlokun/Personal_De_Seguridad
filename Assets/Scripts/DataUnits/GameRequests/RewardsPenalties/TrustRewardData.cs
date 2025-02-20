namespace DataUnits.GameRequests.RewardsPenalties
{
    public class TrustRewardData : RewardData, ITrustRewardData
    {
        private int _mRewardValue;
        public TrustRewardData(RewardTypes rewardType, string[] rewardRawValues) : base(rewardType, rewardRawValues)
        {
            _mRewardValue = int.Parse(rewardRawValues[1]);
        }
        public int TrustAmount => _mRewardValue;
    }
}