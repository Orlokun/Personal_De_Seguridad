using System.Collections;
using Utils;

namespace GameDirection.DayLevelSceneManagers
{
    public interface IIntroSceneOperator : IInitializeWithArg1<IGameDirector>
    {
        public IEnumerator StartIntroScene();
    }
}