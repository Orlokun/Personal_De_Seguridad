using GameDirection.TimeOfDayManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfDayPanelController : MonoBehaviour
{
    [SerializeField] private Button ReturnButton;
    [SerializeField] private Button _mContinueButton;

    private Color veryGoodValueColor = new Color();
    private Color goodValueColor = new Color();
    private Color regularValueColor = new Color();
    private Color badValueColor = new Color();
    private Color veryBadValueColor = new Color();
    
    #region FirstPanelSerializedFields
    [SerializeField] private TMP_Text maxActiveClients;
    [SerializeField] private TMP_Text thiefClients;
    [SerializeField] private TMP_Text clientsCompleted;
    [SerializeField] private TMP_Text clientsDetained;
    [SerializeField] private TMP_Text productsPurchase;
    [SerializeField] private TMP_Text productsStolen;
    [SerializeField] private TMP_Text omniCredits;
    #endregion
    #region SecondPanelSerializedFields
    [SerializeField] private TMP_Text rentOmniCredits;
    [SerializeField] private TMP_Text rentEnergy;
    [SerializeField] private TMP_Text rentSanity;
    [SerializeField] private TMP_Dropdown rentOptionDropdown;
    
    [SerializeField] private TMP_Text foodOmniCredits;
    [SerializeField] private TMP_Text foodEnergy;
    [SerializeField] private TMP_Text foodSanity;
    [SerializeField] private TMP_Dropdown foodOptionDropdown;
    
    [SerializeField] private TMP_Text transportTaxOmniCredits;
    [SerializeField] private TMP_Text transportEnergy;
    [SerializeField] private TMP_Text transportSanity;
    [SerializeField] private TMP_Dropdown transportOptDropdown;

    [SerializeField] private TMP_Text omniTaxOmniCredits;
    [SerializeField] private TMP_Text omniTaxEnergy;
    [SerializeField] private TMP_Text omniTaxSanity;
    
    [SerializeField] private TMP_Text finalOmniCredits;
    #endregion
    
    private bool _isInitialized;
    public bool IsInitialized => _isInitialized;

    private IWorkDayObject _mDisplayedDay;

    private void Awake()
    {
        ClearDisplayedValues();
    }

    #region RentValueChange
    public void OnRentValueChanged()
    {
        var selectedIndex = rentOptionDropdown.value;
        var selectedDayTime = rentOptionDropdown.options[selectedIndex].text;
        switch (selectedDayTime)
        {
            case "Pay":
                SetCurrentRentPaymentValues();
                break;
            case "Don't Pay":
                DontPayRent();
                break;
            default:
                return;
        }
    }
    private void SetCurrentRentPaymentValues()
    {
        rentOmniCredits.text = "-15";
        rentEnergy.text = "-";
        rentSanity.text = "-1";
    }
    private void DontPayRent()
    {
        rentOmniCredits.text = "-";
        rentEnergy.text = "-";
        rentSanity.text = "-2";
    }
    #endregion

    #region FoodValueChange
    public void OnFoodValueChanged()
    {
        var selectedIndex = foodOptionDropdown.value;
        var selectedDayTime = rentOptionDropdown.options[selectedIndex].text;
        switch (selectedDayTime)
        {
            case "FastFood":
                SetFastFoodValues();
                break;
            case "Don't Pay":
                DontPayRent();
                break;
            default:
                return;
        }
    }
    private void SetFastFoodValues()
    {
        foodOmniCredits.text = "-1";
        foodEnergy.text = "+1";
        foodSanity.text = "-3";
    }
    private void SetCookFoodValues()
    {
        foodOmniCredits.text = "-1";
        foodEnergy.text = "0";
        foodSanity.text = "+5";
    }
    private void SetPriceyFoodValues()
    {
        foodOmniCredits.text = "-5";
        foodEnergy.text = "+2";
        foodSanity.text = "+4";
    }
    

    #endregion
    
    public void ClearDisplayedValues()
    {
        _mDisplayedDay = null;
        //First Panel
        maxActiveClients.text = "";
        thiefClients.text = "";
        clientsCompleted.text = "";
        clientsDetained.text = "";
        productsPurchase.text = "";
        productsStolen.text = "";
        omniCredits.text = "";
        
        //Second panel values
        rentOmniCredits.text = "";
        rentEnergy.text = "";
        rentSanity.text = "";
        rentOptionDropdown.value = 0;
        
        foodOmniCredits.text = "";
        foodEnergy.text = "";
        foodSanity.text = "";
        foodOptionDropdown.value = 0;

        transportTaxOmniCredits.text = "";
        transportEnergy.text = "";
        transportSanity.text = "";
        transportOptDropdown.value = 0;
            
        omniTaxOmniCredits.text = "";
        omniTaxEnergy.text = "";
        omniTaxSanity.text = "";
        
        finalOmniCredits.text = "";
    }

    public void SetDayForDisplay(IWorkDayObject dayToDisplay)
    {
        _mDisplayedDay = dayToDisplay;
        UpdateUIObjectsWithDayData();
    }

    private void UpdateUIObjectsWithDayData()
    {
        //First Panel
        maxActiveClients.text = "";
        thiefClients.text = "";
        clientsCompleted.text = "";
        clientsDetained.text = "";
        productsPurchase.text = "";
        productsStolen.text = "";
        omniCredits.text = "";
        
        //Second panel values
        rentOmniCredits.text = "";
        rentEnergy.text = "";
        rentSanity.text = "";
        rentOptionDropdown.value = 0;
        
        foodOmniCredits.text = "";
        foodEnergy.text = "";
        foodSanity.text = "";
        foodOptionDropdown.value = 0;

        transportTaxOmniCredits.text = "";
        transportEnergy.text = "";
        transportSanity.text = "";
        transportOptDropdown.value = 0;
            
        omniTaxOmniCredits.text = "";
        omniTaxEnergy.text = "";
        omniTaxSanity.text = "";
        
        finalOmniCredits.text = "";
    }
}