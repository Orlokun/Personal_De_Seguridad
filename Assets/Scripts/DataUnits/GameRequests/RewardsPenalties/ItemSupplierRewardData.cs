namespace DataUnits.GameRequests.RewardsPenalties
{
    public class ItemSupplierRewardData : RewardData, IItemSupplierRewardData
    {
        private int _mRewardValue;
        public ItemSupplierRewardData(RewardTypes rewardType, string[] rewardRawValues) : base(rewardType, rewardRawValues)
        {
            _mRewardValue = int.Parse(rewardRawValues[1]);
        }
    }
}