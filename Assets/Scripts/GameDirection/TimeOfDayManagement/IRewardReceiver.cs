namespace GameDirection.TimeOfDayManagement
{
    public interface IRewardReceiver
    {
        public void ReceiveOmniCredits(int amount);
        public void ReceiveSeniority(int amount);
    }
}