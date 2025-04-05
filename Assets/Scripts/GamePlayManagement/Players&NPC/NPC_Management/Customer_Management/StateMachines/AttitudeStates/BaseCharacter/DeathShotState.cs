using System;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.BaseCharacter
{
    public class DeathShotState : IAttitudeState
    {
        IBaseCharacterInScene _mCharacter;
        private const string DeathShotAnimation = "DeathShot";
        public DeathShotState(BaseCharacterInScene baseCharacterInScene)
        {
            _mCharacter = baseCharacterInScene;
        }

        public void Enter()
        {
            _mCharacter.ChangeMovementState<IdleMovementState>();   
            _mCharacter.BaseAnimator.ChangeAnimationState(DeathShotAnimation);
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