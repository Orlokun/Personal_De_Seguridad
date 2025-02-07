namespace GamePlayManagement.BitDescriptions.RequestParameters
{
    public enum RequirementActionType
    {
        Hire = 1,
        Use = 2,
        NotUse = 4,
        Buy = 8,
        Capture = 16,
        Punish = 32,
        LetGo = 64,
        Unlock = 128,
        UseSync = 256,
        Trap = 512
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
        Intelligence = 1,
        Kindness = 2,
        Proactivity = 4,
        Aggressive = 8,
        Strength = 16,
        Agility = 32,
        Persuasion = 64,
        Speed = 128,
        FoV = 256,
        Origin = 512,
        BaseType = 1024,
        Quality = 2048,
        Effectiveness = 4096,
        Damage = 8192,
        Range = 16384,
        PeopleInSight = 32768,
        Clarity = 65536,
        JobSupplier = 131072,
        ItemSupplier = 262144,
        ItemValue = 5244288,
    }
}