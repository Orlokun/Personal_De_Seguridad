using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.GuardStates;

namespace GamePlayManagement.ItemManagement.Guards
{
    public interface IGuardStatusModule
    {
        public void SetGuardAttitudeStatus(GuardSpecialAttitudeStatus guardAttitude);
        public bool IsGuardInspecting { get; }
        public GuardSpecialAttitudeStatus CurrentAttitude { get; }

    }
}