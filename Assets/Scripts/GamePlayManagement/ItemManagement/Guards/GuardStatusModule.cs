using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;

namespace GamePlayManagement.ItemManagement.Guards
{
    public class GuardStatusModule : IGuardStatusModule
    {
        private BaseCharacterMovementStatus _currentMovementStatus;
        private GuardSpecialAttitudeStatus _currentAttitudeStatus;
        public GuardSpecialAttitudeStatus GetCurrentStatus => _currentAttitudeStatus;
        
        public void SetGuardAttitudeStatus(GuardSpecialAttitudeStatus guardAttitude)
        {
            switch (guardAttitude)
            {
                case GuardSpecialAttitudeStatus.Idle:
                    break;
                case GuardSpecialAttitudeStatus.Inspecting:
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