using GamePlayManagement;
using Utils;

namespace UI
{
    public interface IInfoCanvasManager : IInitializeWithArg1<IPlayerGameProfile>
    {
        public void UpdateInfo();
    }
}