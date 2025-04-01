namespace ExternalAssets.AdvancedPeopleSystem2.Scripts
{
    public enum CombinerState : byte
    {
        NotCombined,
        InProgressCombineMesh,
        InProgressBlendshapeTransfer,
        InProgressClear,
        Combined,
        UsedPreBuitMeshes
    }
}