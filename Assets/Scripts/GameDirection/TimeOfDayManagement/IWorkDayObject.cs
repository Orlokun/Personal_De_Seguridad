using System.Collections.Generic;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;

namespace GameDirection.TimeOfDayManagement
{
    public interface IWorkDayObject
    {
        public void AddActiveClient();
        public void AddFinishedClient(List<IProductInShelf> productsPurchased);
        public void AddSteal(List<IProductInShelf> productStolen);
        public void AddDetentions();
        
        //GetData
        public DayBitId BitId{get;}
        public int MaxActiveClients{get;}
        public int ThiefClients{get;}
        public int ClientsCompleted{get;}
        public int ClientsDetained{get;}
        public int ProductsPurchased{get;}
        public int ProductsStolen{get;}
        public int TimesStolen{get;}
        public int ValueStolen{get;}
    }
}