using GamePlayManagement;

namespace GameDirection
{
    public class MetaGameDirector : IMetaGameDirector
    {
        private IPlayerGameProfile _mExistingProfile;
        public IPlayerGameProfile GetExistingProfile => _mExistingProfile;
        public void AddNewProfile(IPlayerGameProfile profile)
        {
            _mExistingProfile = null;
        }
    }
}