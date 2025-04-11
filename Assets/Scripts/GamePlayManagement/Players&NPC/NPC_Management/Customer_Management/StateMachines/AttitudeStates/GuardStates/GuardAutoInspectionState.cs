using System.Threading.Tasks;
using GamePlayManagement.ItemManagement.Guards;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.GuardStates
{
    public class GuardAutoInspectionState : IAttitudeState
    {
        private IBaseGuardGameController _mGuard;
        public const string SearchAround = "SearchAround";

        public GuardAutoInspectionState(IBaseGuardGameController mGuard)
        {
            this._mGuard = mGuard;
        }

        public void Enter()
        {
            _mGuard.SetGuardDestination(_mGuard.GetInspectionModule.GetCurrentPosition.Position);
            _mGuard.WalkingDestinationReached += WalkingDestinationReached;
        }

        public void Exit()
        {
            _mGuard.WalkingDestinationReached -= WalkingDestinationReached;
        }

        public void Update()
        {
            
        }

        public void WalkingDestinationReached()
        {
            ReachInspectedZone();
        }
        //TODO: Hardcoded numbers should depend on guard statistics.
        private async void ReachInspectedZone()
        {
            var nextPosition = _mGuard.PositionsManager.GetNextPosition(_mGuard.CurrentInspectionPosition.Id);
            _mGuard.ChangeMovementState<IdleMovementState>();
            await Task.Delay(500);
            _mGuard.BaseAnimator.ChangeAnimationState(SearchAround);
            await Task.Delay(4000); 
            _mGuard.GetInspectionModule.SetNewCurrentPosition(nextPosition);
            _mGuard.SetGuardDestination(_mGuard.GetInspectionModule.GetCurrentPosition.Position);
            _mGuard.ChangeMovementState<WalkingState>();
        }
    }
}