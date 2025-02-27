namespace GamePlayManagement.GameRequests.RewardsPenalties
{
    public class SeniorityRewardData : RewardData, ISeniorityRewardData
    {
        private int _mRewardValue;
        public int SeniorityRewardAmount => _mRewardValue;
        public SeniorityRewardData(RewardTypes rewardType, string[] rewardRawValues) : base(rewardType, rewardRawValues)
        {
            _mRewardValue = int.Parse(rewardRawValues[1]);
        }

    }
}