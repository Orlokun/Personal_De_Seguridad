namespace GamePlayManagement.BitDescriptions.RequestParameters
{
    public enum RequirementConsideredParameter
    {
        None = 1 << 0,
        Origin = 1 << 1,
        BaseType = 1 << 2,
        Quality = 1 << 3,
        JobSupplier = 1 << 4,
        ItemSupplier = 1 << 5,
        ItemValue = 1 << 6,
        Variable = 1 << 7,
        OmniCreditsAmount = 1 << 8,
    }
}