using System;
using DialogueSystem;

namespace GameDirection.TimeOfDayManagement
{
    public interface IPlayerGameStatusModule : IPlayerGameStatusModuleData, IRewardReceiver
    {
        public void PlayerLostGame(EndingTypes ending);
        public void DoCigarAction(Tuple<int, int> cigarCost);
    }
}