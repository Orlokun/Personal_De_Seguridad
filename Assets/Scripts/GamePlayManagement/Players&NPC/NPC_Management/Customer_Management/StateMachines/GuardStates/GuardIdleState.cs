using System;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.GuardStates
{
    public class GuardIdleState : IAttitudeState
    {
        private BaseCharacterInScene _mGuardCharacter;

        public GuardIdleState(BaseCharacterInScene baseCharacterInScene)
        {
            _mGuardCharacter = baseCharacterInScene;
        }

        public void Enter()
        {
            
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void WalkingDestinationReached()
        {
            throw new NotImplementedException();
        }
    }
}