using System.Collections;
using GameDirection.TimeOfDayManagement;
using Utils;

namespace GameDirection.DayLevelSceneManagers
{
    public interface ILevelDayManager : IInitializeWithArg2<IGameDirector, DayBitId>
    {
        public IEnumerator StartDayManagement();
        public event DayLevelSceneManagement.FinishCurrentDialogue OnFinishCurrentDialogue;
    }
}