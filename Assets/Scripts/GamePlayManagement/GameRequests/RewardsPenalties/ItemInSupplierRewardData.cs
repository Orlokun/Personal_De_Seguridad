namespace GamePlayManagement.GameRequests.RewardsPenalties
{
    public class ItemInSupplierRewardData : RewardData, IItemInSupplierRewardData
    {
        private int _mRewardValue;
        public ItemInSupplierRewardData(RewardTypes rewardType, string[] rewardRawValues) : base(rewardType, rewardRawValues)
        {
            _mRewardValue = int.Parse(rewardRawValues[1]);
        }
    }
}