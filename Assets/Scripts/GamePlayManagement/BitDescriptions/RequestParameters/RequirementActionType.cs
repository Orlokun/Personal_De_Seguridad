namespace GamePlayManagement.BitDescriptions.RequestParameters
{
    public enum RequirementActionType
    {
        Hire = 1 << 1,
        Use = 1<<2,
        NotUse = 1<<3,
        Buy = 1<<4,
        Capture = 1<<5,
        Punish = 1<<6,
        LetGo = 1<<7,
        Unlock = 1<<8,
        UseSync = 1<<9,
        Trap = 1<<10,
    }
}