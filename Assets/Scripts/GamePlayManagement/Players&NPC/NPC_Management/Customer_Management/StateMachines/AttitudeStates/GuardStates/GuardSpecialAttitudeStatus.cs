using System;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.GuardStates
{
    [Flags]
    public enum GuardSpecialAttitudeStatus
    {
        Idle,
        Slacking,
        ManualInspecting,
        Inspecting, 
        Communicating,
        Following,
        Chasing,
        Tackling,
        Fighting,
    }
}