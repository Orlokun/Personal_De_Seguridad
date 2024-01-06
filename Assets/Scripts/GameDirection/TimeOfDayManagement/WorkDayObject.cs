using System.Collections.Generic;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;

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
        protected int MProductsPurchasedValue;
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
        public int ProductsPurchased => MProductsPurchasedValue;
        public int ProductsStolen => MProductsStolen;
        public int TimesStolen => MTimesStolen;
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
        public void AddActiveClient()
        {
            _activeClients++;
            if (_activeClients > MMaxActiveClients)
            {
                MMaxActiveClients = _activeClients;
            }
            //Manage stress eventually
        }
        public void AddFinishedClient(List<IStoreProductObjectData> productsPurchased)
        {
            foreach (var productInShelf in productsPurchased)
            {
                MProductsPurchasedValue += productInShelf.Price;
            }
            _activeClients--;
            MClientsCompleted++;
        }
        public void AddSteal(List<IStoreProductObjectData> productStolen)
        {
            foreach (var productInShelf in productStolen)
            {
                MValueStolen += productInShelf.Price;
            }
            
            MTimesStolen++;
        }
        public void AddDetentions()
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePartOfDay(PartOfDay newPartOfDay)
        {
            _currentPartOfDay = newPartOfDay;
        }
    
    }
}