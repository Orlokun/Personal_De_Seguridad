using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.BaseCharacter
{
    public class ShootTargetState : IAttitudeState
    {
        IBaseCharacterInScene _mCharacter;
        private const string ShootRifleAnim = "TargetRifleShoot";
        public ShootTargetState(BaseCharacterInScene baseCharacterInScene)
        {
            _mCharacter = baseCharacterInScene;
        }

        public void Enter()
        {
            _mCharacter.ChangeMovementState<IdleMovementState>();   
            _mCharacter.BaseAnimator.ChangeAnimationState(ShootRifleAnim);
        }

        public void Exit()
        {
        }

        public void Update()
        {
        }

        public void WalkingDestinationReached()
        {
        }
    }
}