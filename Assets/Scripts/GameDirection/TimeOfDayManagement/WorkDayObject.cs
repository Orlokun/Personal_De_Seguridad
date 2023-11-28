using System.Collections.Generic;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;

namespace GameDirection.TimeOfDayManagement
{
    public abstract class WorkDayObjectData
    {
        protected DayBitId MBitId;
        protected int MMaxActiveClients;
        protected int MThiefClients;
        protected int MClientsCompleted;
        protected int MClientsDetained;
        protected int MProductsPurchasedValue;
        protected int MProductsStolen;
        protected int MTimesStolen;
        protected int MValueStolen;
    }
    public class WorkDayObject : WorkDayObjectData, IWorkDayObject
    {
        private int _activeClients;
        public DayBitId BitId => MBitId;
        public int MaxActiveClients => MMaxActiveClients;
        public int ThiefClients => MThiefClients;
        public int ClientsCompleted => MClientsCompleted;
        public int ClientsDetained => MClientsDetained;
        public int ProductsPurchased => MProductsPurchasedValue;
        public int ProductsStolen => MProductsStolen;
        public int TimesStolen => MTimesStolen;
        public int ValueStolen => MValueStolen;
        public WorkDayObject(DayBitId mBitId)
        {
            MBitId = mBitId;
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
        public void AddFinishedClient(List<IProductInShelf> productsPurchased)
        {
            foreach (var productInShelf in productsPurchased)
            {
                MProductsPurchasedValue += productInShelf.Price;
            }
            _activeClients--;
            MClientsCompleted++;
        }
        public void AddSteal(List<IProductInShelf> productStolen)
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


    }
}