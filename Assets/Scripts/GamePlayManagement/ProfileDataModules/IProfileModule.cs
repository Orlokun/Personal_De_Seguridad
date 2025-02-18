namespace GamePlayManagement.ProfileDataModules
{
    public interface IProfileModule
    {
        public void SetProfile(IPlayerGameProfile currentPlayerProfile);
        public void PlayerLostResetData();
    }
}