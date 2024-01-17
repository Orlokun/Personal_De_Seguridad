using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

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
        protected int MProductsStolen;
        protected int MTimesStolen;
        protected int MValueStolen;
        protected int MOmniCreditsEarnedEarned;
    }
    public class WorkDayObject : WorkDayObjectData, IWorkDayObject
    {
        private int _activeClients;
        private PartOfDay _currentPartOfDay;


        public DayBitId BitId => MBitId;
        public JobSupplierBitId JobSupplierId => MJobSupplierId;
        public int MaxActiveClients => MMaxActiveClients;
        public int ThiefClients => MThiefClients;
        public int ClientsCompleted => MClientsCompleted;
        public int ClientsDetained => MClientsDetained;
        public int ProductsPurchased => MValuePurchased;
        public int ProductsStolen => MProductsStolen;
        public int TimesStolen => MTimesStolen;
        public int ValuePurchased => MValuePurchased;
        public int ValueStolen => MValueStolen;
        public int OmniCreditsEarned => MOmniCreditsEarnedEarned;
        public PartOfDay CurrentPartOfDay => _currentPartOfDay;
        public WorkDayObject(DayBitId mBitId)
        {
            MBitId = mBitId;
            _currentPartOfDay = PartOfDay.EarlyMorning;
        }
        public void SetJobSupplier(JobSupplierBitId newJobSupplier)
        {
            MJobSupplierId = newJobSupplier;
        }
        private void AddActiveClient()
        {
            _activeClients++;
            if (_activeClients > MMaxActiveClients)
            {
                MMaxActiveClients = _activeClients;
            }
            //Manage stress eventually
        }
        private void RemoveActiveClient()
        {
            _activeClients--;
            //Manage stress eventually
        }

        private void AddPurchase(int valuePurchased)
        {
            MValuePurchased += valuePurchased;
        }
        
        private void AddSteal(int valueStolen)
        {
            MValueStolen += valueStolen;
            if (valueStolen > 0)
            {
                MTimesStolen++;
            }
        }
        public void AddDetentions()
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePartOfDay(PartOfDay newPartOfDay)
        {
            _currentPartOfDay = newPartOfDay;
        }

        public void UpdateCustomerAdded(IBaseCustomer newcustomerCount)
        {
            AddActiveClient();
        }

        public void UpdateCustomerRemoved(ICustomerPurchaseStealData customerData)
        {
            RemoveActiveClient();
            AddSteal(customerData.StolenProductsValue);
            AddPurchase(customerData.PurchasedProductsValue);
        }
    }
}