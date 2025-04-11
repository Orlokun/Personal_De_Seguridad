namespace GamePlayManagement.Players_NPC.NPC_Management.SpecialNPC
{
    public class CutSceneShooterCharacter : BaseCharacterInScene
    {
        private void Update()
        {
            _mAttitudeStateMachine.Update();
            _mMovementStateMachine.Update();
        }
    }
}