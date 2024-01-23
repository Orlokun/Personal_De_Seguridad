using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;

namespace GamePlayManagement.ItemManagement.Guards
{
    public class GuardStatusModule : IGuardStatusModule
    {
        private BaseCharacterMovementStatus _currentMovementStatus;
        private GuardSpecialAttitudeStatus _currentAttitudeStatus;
        private IBaseGuardGameObject _mGuardObject;
        public GuardStatusModule(IBaseGuardGameObject guardObject)
        {
            _mGuardObject = guardObject;
        }

        public bool IsGuardInspecting => CurrentAttitude == GuardSpecialAttitudeStatus.Inspecting;
        public GuardSpecialAttitudeStatus CurrentAttitude => _currentAttitudeStatus;

        public void SetGuardAttitudeStatus(GuardSpecialAttitudeStatus guardAttitude)
        {
            _currentAttitudeStatus = guardAttitude;
            switch (guardAttitude)
            {
                case GuardSpecialAttitudeStatus.Idle:
                    break;
                case GuardSpecialAttitudeStatus.Inspecting:
                    _mGuardObject.GetNavMeshAgent.SetDestination(_mGuardObject.CurrentInspectionPosition.Position);
                    break;
                case GuardSpecialAttitudeStatus.Slacking:
                    break;
                case GuardSpecialAttitudeStatus.Following:
                    break;
                case GuardSpecialAttitudeStatus.Chasing:
                    break;
            }
        }
    }
}