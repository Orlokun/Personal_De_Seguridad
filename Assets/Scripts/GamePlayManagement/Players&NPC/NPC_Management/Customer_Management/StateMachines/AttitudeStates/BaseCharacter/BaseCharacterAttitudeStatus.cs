using System;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates.BaseCharacter
{
    [Flags]
    public enum BaseCharacterAttitudeStatus
    {
        Idle = 1,
        Fighting,
        Stealing,
        Shopping,
        Paying,
        EvaluatingProduct,
        WonderingAround,
        Screaming,
        Talking,
        ScaredCrouch,
        ScaredRunningAway,
    }
}