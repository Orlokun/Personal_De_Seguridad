using System;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management
{
    [Flags]
    public enum BaseCharacterMovementStatus
    {
        Idle = 1,
        Walking = 2, 
        EvaluatingProduct = 4,
        Running = 8,
        Dancing = 16,
        Sitting = 64,
        Falling = 128,
        
    }
    
    [Flags]
    public enum BaseCustomerAttitudeStatus
    {
        Neutral = 1,
        Fighting = 2,
        Stealing = 4,
        Shopping = 8,
        Paying = 16,
        Leaving = 32,
        Entering = 64,
        EvaluatingProduct = 128
    }
}