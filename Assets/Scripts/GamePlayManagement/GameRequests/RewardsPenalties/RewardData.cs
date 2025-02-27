namespace GamePlayManagement.GameRequests.RewardsPenalties
{
    public class RewardData : IRewardData
    {
        private RewardTypes _mRewardType;
        private string[] _mRewardRawValues;
        public RewardTypes RewardType => _mRewardType;
        public string[] RewardRawValue => _mRewardRawValues;
        public RewardData(RewardTypes rewardType, string[] rewardRawValues)
        {
            _mRewardType = rewardType;
            _mRewardRawValues = rewardRawValues;
        }
    }
}