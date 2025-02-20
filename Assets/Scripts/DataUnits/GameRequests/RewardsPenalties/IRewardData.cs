namespace DataUnits.GameRequests.RewardsPenalties
{
    public interface IRewardData
    {
        public RewardTypes RewardType { get; }
        public string[] RewardRawValue { get; }
    }
}