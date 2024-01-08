using System.Collections.Generic;
using GamePlayManagement.BitDescriptions.Suppliers;
using GamePlayManagement.LevelManagement.LevelObjectsManagement;
using Players_NPC.NPC_Management.Customer_Management;
using Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;

namespace GameDirection.TimeOfDayManagement
{
    public interface IWorkDayObject : ICustomerManagementObserver
    {
        //Day GamePlay Management
        public void AddDetentions();
        public void UpdatePartOfDay(PartOfDay newPartOfDay);

        public void SetJobSupplier(JobSupplierBitId newJobSupplier);
        
        //Day Base Data
        public DayBitId BitId{get;}
        public JobSupplierBitId JobSupplierId {get;}
        public PartOfDay CurrentPartOfDay { get; }
        public int MaxActiveClients{get;}
        public int ThiefClients{get;}
        public int ClientsCompleted{get;}
        public int ClientsDetained{get;}
        public int ProductsPurchased{get;}
        public int ProductsStolen{get;}
        public int TimesStolen{get;}
        public int ValueStolen{get;}
        public int ValuePurchased { get; }
        public int OmniCreditsEarned { get; }
    }
}