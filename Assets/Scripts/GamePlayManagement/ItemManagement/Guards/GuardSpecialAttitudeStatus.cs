using System;

namespace GamePlayManagement.ItemManagement.Guards
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