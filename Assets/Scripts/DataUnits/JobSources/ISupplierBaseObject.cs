namespace DataUnits.JobSources
{
    public interface ISupplierBaseObject
    {
        public int StorePhoneNumber { get; set; }
        public string StoreName { get; set; }
        public string StoreOwnerName { get; set; }
        public void PlayerLostResetData();
    }
}