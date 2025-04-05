namespace GamePlayManagement.Players_NPC
{
    public class CutSceneCharacter : BaseCharacterInScene
    {
        private void Update()
        {
            _mAttitudeStateMachine.Update();
            _mMovementStateMachine.Update();
        }
    }
}