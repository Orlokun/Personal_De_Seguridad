using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

namespace GameDirection.TimeOfDayManagement
{
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
        public int ProductsPurchased => MProductsPurchased;
        public int ProductsStolen => MProductsStolen;
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
        private void SustractActiveClient()
        {
            _activeClients--;
            //Manage stress eventually
        }

        private void AddPurchase(int valuePurchased)
        {
            MValuePurchased += valuePurchased;
        }
        
        private void AddStealAmount(int valueStolen)
        {
            MValueStolen += valueStolen;
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
        private void CompleteClient()
        {
            MClientsCompleted++;
        }
        public void UpdateCustomerRemoved(IBaseCustomer customerBaseData)
        {
            var customerData = customerBaseData.GetCustomerStoreVisitData;
            if (customerData.StolenProductsValue > 0)
            {
                AddThiefClientsCount();
                //Process Theft Completed Event
            }
            SustractActiveClient();
            MProductsStolen += customerData.GetStolenProductsCount;
            MValuePurchased += customerData.GetPurchasedProductsCount;

            AddPurchase(customerData.PurchasedProductsValue);
            AddStealAmount(customerData.StolenProductsValue);

            CompleteClient();
        }

        private void AddThiefClientsCount()
        {
            MThiefClients++;
        }
    }
}