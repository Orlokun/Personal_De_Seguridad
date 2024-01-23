using GamePlayManagement;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using TMPro;
using UnityEngine;
using Utils;

namespace UI
{
    public interface IInfoCanvasManager : IInitializeWithArg1<IPlayerGameProfile>
    {
        public void UpdateBudget(int newIncome);
        public void UpdateEarnings(int newEarnings);
        public void UpdateLosses(int newLosses);
        public void UpdateInfo();
    }
    public class InfoCanvasManager : MonoBehaviour, IInfoCanvasManager, ICustomerManagementObserver
    {
        [SerializeField] private TMP_Text activeCustomers;
        [SerializeField] private TMP_Text budget;
        [SerializeField] private TMP_Text earnings;
        [SerializeField] private TMP_Text losses;
        [SerializeField] private TMP_Text personalBudget;
        [SerializeField] private TMP_Text health;

        [SerializeField] private GameObject horizontalInfoPanel;

        private int _activeCustomers = 0;
        private IPlayerGameProfile _mPlayerProfile;
        // Start is called before the first frame update
        void Start()
        {
            activeCustomers.text = _activeCustomers.ToString();
            earnings.text = "0";
            losses.text = "0";
        }

        private bool _mIsInitialized;
        public bool IsInitialized => _mIsInitialized;

        public void Initialize(IPlayerGameProfile injectionClass)
        {
            if (IsInitialized)
            {
                return;
            }
            var customerManager = (ICustomersInSceneManager)FindObjectOfType <CustomersInSceneManager>();
            customerManager.RegisterObserver(this);
            
            _mPlayerProfile = injectionClass;
            UpdateInfo();
            _mIsInitialized = true;
        }

        private void UpdateStoreData()
        {
            if (_mPlayerProfile.GetActiveJobsModule().CurrentEmployer == 0)
            {
                return;
            }
            var employerData = _mPlayerProfile.GetActiveJobsModule().CurrentEmployerData();
            var workDay = _mPlayerProfile.GetProfileCalendar().GetCurrentWorkDayObject();
            budget.text = employerData.Budget.ToString();
            earnings.text = workDay.ValuePurchased.ToString();
            losses.text = workDay.ValueStolen.ToString();
        }

        private void UpdateActiveClientsUI(int newActiveCustomers)
        {
            activeCustomers.text = newActiveCustomers.ToString();
        }
        
        public void UpdateBudget(int newIncome)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateEarnings(int newEarnings)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateLosses(int newLosses)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateInfo()
        {
            UpdateProfileData();
            UpdateStoreData();
        }
        private void UpdateProfileData()
        {
            personalBudget.text = _mPlayerProfile.GetStatusModule().PlayerOmniCredits.ToString();
            health.text = _mPlayerProfile.GetStatusModule().PlayerHealth.ToString();
        }
        public void UpdateCustomerAdded(IBaseCustomer newCustomerCount)
        {
            _activeCustomers++;
            UpdateActiveClientsUI(_activeCustomers);
        }
        public void UpdateCustomerRemoved(ICustomerPurchaseStealData customerData)
        {
            _activeCustomers--;
            UpdateActiveClientsUI(_activeCustomers);
            UpdateEarningsData(customerData);
        }

        private void UpdateEarningsData(ICustomerPurchaseStealData customerData)
        {
            var stolenAmount = customerData.StolenProductsValue;
            var purchasedAmount = customerData.PurchasedProductsValue;

            var hasCurrentEarnings= int.TryParse(earnings.text, out var result);
            var newEarnings = result + purchasedAmount;
            earnings.text = newEarnings.ToString();
            
            var hasCurrentSteal= int.TryParse(losses.text, out var stealResult);
            var newLosses = stealResult + stolenAmount;
            losses.text = newLosses.ToString();
        }
    }
}