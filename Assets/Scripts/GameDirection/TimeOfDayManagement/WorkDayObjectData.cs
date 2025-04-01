using GamePlayManagement.BitDescriptions.Suppliers;

namespace GameDirection.TimeOfDayManagement
{
    public abstract class WorkDayObjectData
    {
        protected DayBitId MBitId;
        protected JobSupplierBitId MJobSupplierId;
        protected int MMaxActiveClients;
        protected int MThiefClients;
        protected int MClientsCompleted;
        protected int MClientsDetained;
        protected int MValuePurchased;
        protected int MProductsPurchased;
        protected int MProductsStolen;
        protected int MValueStolen;
        protected int MOmniCreditsEarnedEarned;
    }
}