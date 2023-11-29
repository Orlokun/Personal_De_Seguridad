using GameDirection;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement.BitDescriptions;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class EndOfDayPanelController : MonoBehaviour, IInitialize, IEndOfDayPanelController
{
    [SerializeField] private Button ReturnButton;
    [SerializeField] private Button _mContinueButton;

    private Color veryGoodValueColor = new Color();
    private Color goodValueColor = new Color();
    private Color regularValueColor = new Color();
    private Color badValueColor = new Color();
    private Color veryBadValueColor = new Color();

    private bool firstOpenPanelOne;
    private bool firstOpenPanelTwo;

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
    
    [SerializeField] private TMP_Text transportOmniCredits;
    [SerializeField] private TMP_Text transportEnergy;
    [SerializeField] private TMP_Text transportSanity;
    [SerializeField] private TMP_Dropdown transportOptDropdown;

    [SerializeField] private TMP_Text omniTaxOmniCredits;
    [SerializeField] private TMP_Text omniTaxEnergy;
    [SerializeField] private TMP_Text omniTaxSanity;
    
    [SerializeField] private TMP_Text endOfDayCreditsStart;
    [SerializeField] private TMP_Text endOfDayCreditsEnd;
    
    
    #endregion
    
    private bool _isInitialized;
    public bool IsInitialized => _isInitialized;
    private static IEndOfDayPanelController _mInstance;
    public static IEndOfDayPanelController Instance => _mInstance;
    
    public void Initialize()
    {
        if (_mInstance != null && (EndOfDayPanelController) _mInstance != this)
        {
            Destroy(this);
        }
        _mInstance = this;
        ClearDisplayedValues();
        _mContinueButton.onClick.AddListener(GoToSecondPanel);
        ReturnButton.onClick.AddListener(ReturnToFirstPanel);
        OnRentValueChanged();
        OnFoodValueChanged();
        OnTransportValueChange();
        _isInitialized = true;
    }

    private IWorkDayObject _mDisplayedDay;

    private void Awake()
    {
        Initialize();
    }
    private void ReturnToFirstPanel()
    {
        GameDirector.Instance.GetUIController.DeactivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.SECOND_PANEL);
        GameDirector.Instance.GetUIController.ActivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.FIRST_PANEL);
        _mContinueButton.onClick.RemoveAllListeners();
        _mContinueButton.onClick.AddListener(GoToSecondPanel);
    }
    private void GoToSecondPanel()
    {
        Debug.Log("GOING TO SECOND PANEL!");
        GameDirector.Instance.GetUIController.DeactivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.FIRST_PANEL);
        GameDirector.Instance.GetUIController.ActivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.SECOND_PANEL);
        _mContinueButton.onClick.RemoveAllListeners();
        _mContinueButton.onClick.AddListener(ConfirmAndFinishEndOfDay);
    }
    private void ConfirmAndFinishEndOfDay()
    {
        //Calculate decisions
        //Save decisions to day
        //Advance to Next Day in data
        //Load new level and New Day
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
        var selectedDayTime = foodOptionDropdown.options[selectedIndex].text;
        switch (selectedDayTime)
        {
            case "Fast Food":
                SetFastFoodValues();
                break;
            case "Restaurant":
                SetRestaurantFoodValues();
                break;            
            case "Cook":
                SetCookFoodValues();
                break;
            default:
                return;
        }
    }
    //TODO: This should be data downloaded from server. Create a ticket. 
    private void SetFastFoodValues()
    {
        foodOmniCredits.text = "-1";
        foodEnergy.text = "+1";
        foodSanity.text = "-3";
    }
    private void SetRestaurantFoodValues()
    {
        foodOmniCredits.text = "-5";
        foodEnergy.text = "+2";
        foodSanity.text = "+4";
    }
    private void SetCookFoodValues()
    {
        foodOmniCredits.text = "-1";
        foodEnergy.text = "0";
        foodSanity.text = "+5";
    }
    #endregion
    
    #region TransportValueChange
    public void OnTransportValueChange()
    {
        var selectedIndex = transportOptDropdown.value;
        var selectedDayTime = transportOptDropdown.options[selectedIndex].text;
        switch (selectedDayTime)
        {
            case "Bike":
                SetTransportBikeValues();
                break;
            case "Bus":
                SetTransportBusValues();
                break;            
            default:
                return;
        }
    }
    //TODO: This should be data downloaded from server. Create a ticket. 
    private void SetTransportBikeValues()
    {
        transportOmniCredits.text = "0";
        transportEnergy.text = "+1";
        transportSanity.text = "+3";
    }
    private void SetTransportBusValues()
    {
        transportOmniCredits.text = "-2";
        transportEnergy.text = "-2";
        transportSanity.text = "-2";
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

        transportOmniCredits.text = "";
        transportEnergy.text = "";
        transportSanity.text = "";
        transportOptDropdown.value = 0;
            
        omniTaxOmniCredits.text = "";
        omniTaxEnergy.text = "";
        omniTaxSanity.text = "";
        
        endOfDayCreditsStart.text = "";
    }

    public void SetDayForDisplay(IWorkDayObject dayToDisplay)
    {
        _mDisplayedDay = dayToDisplay;
        UpdateUIObjectsWithDayData();
    }

    private void UpdateUIObjectsWithDayData()
    {
        //First Panel
        maxActiveClients.text = _mDisplayedDay.MaxActiveClients.ToString();
        thiefClients.text = _mDisplayedDay.ThiefClients.ToString();
        clientsCompleted.text = _mDisplayedDay.ClientsCompleted.ToString();
        clientsDetained.text = _mDisplayedDay.ClientsDetained.ToString();
        productsPurchase.text = _mDisplayedDay.ProductsPurchased.ToString();
        productsStolen.text = _mDisplayedDay.ProductsStolen.ToString();
        omniCredits.text = _mDisplayedDay.OmniCreditsEarned.ToString();
        
        //Second panel values
        rentOmniCredits.text = "";
        rentEnergy.text = "";
        rentSanity.text = "";
        rentOptionDropdown.value = 0;
        
        foodOmniCredits.text = "";
        foodEnergy.text = "";
        foodSanity.text = "";
        foodOptionDropdown.value = 0;

        transportOmniCredits.text = "";
        transportEnergy.text = "";
        transportSanity.text = "";
        transportOptDropdown.value = 0;
            
        omniTaxOmniCredits.text = "";
        omniTaxEnergy.text = "";
        omniTaxSanity.text = "";
        
        endOfDayCreditsStart.text = "";
    }
}