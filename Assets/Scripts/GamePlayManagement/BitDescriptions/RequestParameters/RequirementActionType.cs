namespace GamePlayManagement.BitDescriptions.RequestParameters
{
    public enum ComplianceMotivationalLevel
    {
        Forbidden, 
        Discouraged,
        Neutral,
        Encouraged,
        Mandatory
    }
    public enum ComplianceActionType
    {
        Use, 
        Trap,
        Retain,
        Punish,
        Torture,
        Kill,   
        Accept,
        KickOut
    }
    
    public enum ComplianceObjectType
    {
        Smoke,
        DoorLock,
        Phone,  
        Bribe,
        Client,
        Guard,
        Camera,
        Weapon,
        Traps,
        Other,
        AnyItem,
    }
    
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

    public enum RequirementLogicEvaluator
    {
        EqLarger = 1,
        EqSmaller,
        Equal,
        NotEqual
    }

    public enum RequirementObjectType
    {
        JobSupplier, 
        ItemSupplier,
        AnyItem,
        Guard,
        Camera,
        Weapon,
        Traps,
        Other,
        Clients,
    }

    public enum RequirementConsideredParameter
    {
        None = 1 << 0,
        Intelligence = 1 << 1,
        Kindness = 1 << 2,
        Proactivity = 1 <<3,
        Aggressive = 1 << 4,
        Strength = 1 << 5,
        Agility = 1 << 6,
        Persuasion = 1 << 7,
        Speed = 1 << 8,
        FoV = 1 << 9,
        Origin = 1 << 10,
        BaseType = 1 << 11,
        Quality = 1 << 12,
        Effectiveness = 1 << 13,
        Damage = 1 << 14,
        Range = 1 << 15,
        PeopleInSight = 1 << 16,
        Clarity = 1 << 17,
        JobSupplier = 1 << 18,
        ItemSupplier = 1 << 19,
        ItemValue = 1 << 20
    }
}