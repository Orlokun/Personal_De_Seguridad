using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.GameRequests.RewardsPenalties
{
    public interface IItemInSupplierRewardData
    {
    }
    public class ItemInSupplierRewardData : RewardData, IItemInSupplierRewardData
    {
        private int _mRewardValue;
        public ItemInSupplierRewardData(RewardTypes rewardType, string[] rewardRawValues) : base(rewardType, rewardRawValues)
        {
            _mRewardValue = int.Parse(rewardRawValues[1]);
        }
    }
}