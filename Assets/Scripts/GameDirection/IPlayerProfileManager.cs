using GameManagement;
using GamePlayManagement;

namespace GameDirection
{
    public interface IPlayerProfileManager
    {
        public IPlayerGameProfile GetActiveGameProfile { get; }
    }
}