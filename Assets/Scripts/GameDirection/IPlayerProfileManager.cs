using GameManagement;
namespace GameDirection
{
    public interface IPlayerProfileManager
    {
        public IPlayerGameProfile GetActiveGameProfile { get; }
    }
}