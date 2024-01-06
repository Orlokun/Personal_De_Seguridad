namespace Players_NPC.NPC_Management.Customer_Management
{
    public interface ICustomerManagementObserver
    {
        public void UpdateCustomerAdded(IBaseCustomer newcustomerCount);
        public void UpdateCustomerRemoved(IBaseCustomer customerData);
    }
}