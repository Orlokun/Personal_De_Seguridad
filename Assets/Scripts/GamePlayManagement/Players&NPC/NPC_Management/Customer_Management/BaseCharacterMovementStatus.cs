using System;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    [Flags]
    public enum BaseCharacterMovementStatus
    {
        Idle = 1,
        Walking = 2, 
        Rotating = 4,
        Running = 8,
        Dancing = 16,
        Sitting = 64,
        Falling = 128,
    }
    

    
    
}