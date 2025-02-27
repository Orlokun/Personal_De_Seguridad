namespace GamePlayManagement.GameRequests.RewardsPenalties
{
    public interface IPenaltyData
    {
        public RewardTypes RewardType { get; }
        public int RewardValue { get; }
    }
}