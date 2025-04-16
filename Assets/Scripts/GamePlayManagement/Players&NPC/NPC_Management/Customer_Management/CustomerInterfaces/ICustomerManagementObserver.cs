namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomerManagementObserver
    {
        public void UpdateCustomerAdded(IBaseCustomer newcustomerCount);
        public void UpdateCustomerRemoved(IBaseCustomer customerData);
    }
}