using System.Collections.Generic;
using System.Threading.Tasks;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomerDataLoader
    {
        Task LoadCustomerManagementDataAsync(string url);
        Dictionary<JobSupplierBitId, ICustomersInstantiationFlowData> GetCustomerManagementData();
    }
}