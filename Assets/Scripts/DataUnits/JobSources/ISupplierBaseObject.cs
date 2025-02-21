namespace DataUnits.JobSources
{
    public interface ISupplierBaseObject : IFondnessCharacter
    {
        public int StorePhoneNumber { get; set; }
        public string StoreName { get; set; }
        public string StoreOwnerName { get; set; }
        public void PlayerLostResetData();
    }
}