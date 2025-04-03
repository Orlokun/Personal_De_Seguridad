using System.Collections;
using Utils;

namespace GameDirection.DayLevelSceneManagers
{
    public interface IIntroSceneOperator : IInitializeWithArg2<IGameDirector, IIntroSceneInGameManager>
    {
        public IEnumerator StartIntroScene();
        public void StartTimer();
    }
}