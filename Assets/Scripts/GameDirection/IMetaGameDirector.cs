using GamePlayManagement;

namespace GameDirection
{
    public interface IMetaGameDirector
    {
        public IPlayerGameProfile GetExistingProfile { get; }
        public void AddNewProfile(IPlayerGameProfile profle);
    }
}