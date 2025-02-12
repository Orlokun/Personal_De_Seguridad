namespace DataUnits.GameCatalogues.JsonModels
{
    public class JobSupplierData
    {
        public int JobSupplierBitId { get; set; }
        public string StoreType { get; set; }
        public string StoreName { get; set; }
        public string StoreOwnerName { get; set; }
        public int StoreUnlockPoints { get; set; }
        public string StoreDescription { get; set; }
        public int StorePhoneNumber { get; set; }
        public int SpeakerIndex { get; set; }
        public int[] StoreMinMaxClients { get; set; }
        public int Sanity { get; set; }
        public int Kindness { get; set; }
        public int Violence { get; set; }
        public int Intelligence { get; set; }
        public int Money { get; set; }
        public string SpriteName { get; set; }
        public int StoreOwnerAge { get; set; }
        public int Budget { get; set; }
    }

}