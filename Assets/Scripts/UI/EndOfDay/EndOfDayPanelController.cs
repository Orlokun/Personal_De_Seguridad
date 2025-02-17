using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using GameDirection;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.EndOfDay
{
    public class EndOfDayPanelController : MonoBehaviour, IEndOfDayPanelController
    {
        #region PrivateMembers
        [SerializeField] private Button mReturnButton;
        [SerializeField] private Button mContinueButton;

        private Color _veryGoodValueColor = Color.green;
        private Color _goodValueColor = new Color();
        private Color _regularValueColor = new Color();
        private Color _badValueColor = new Color();
        private Color _veryBadValueColor = Color.red;
    
        private IWorkDayObject _mDisplayedDay;
        private IRentValuesCatalogue _mRentCatalogue;
        private IFoodValuesCatalogue _mFoodCatalogue;
        private ITransportValuesCatalogue _mTransportCatalogue;
        private IPlayerGameProfile _currentPlayer;
        #endregion

        #region PrivateConstants
        #region FoodNames
        private const string FastFoodName="Fast Food";
        private const string SimpleCookName="Simple Home Cook";
        private const string CoffeeSnacksName="Cofee & Snacks";
        private const string CompleteCookName="Complete Home Cook";
        private const string StreetFoodName="Street Food";
        private const string GourmetFoodName="Gourmet Takeout";
        #endregion

        #region TransportNames
        private const string OmniBikeName ="OmniBikeCab";
        private const string OmniCabName ="OmniCab";
        private const string OmniBusName ="OmniBus";
        private const string OmniHuberName ="OmniHuber";
        private const string OmniDriverName ="OmniDriver";
        private const string OmniCopterName ="Omnicopter";
        private const string OmniWalkName ="OmniWalk";
        #endregion
        #endregion
    
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

        #region Singleton
        private bool _mInitialized;
        public bool MInitialized => _mInitialized;
        private static IEndOfDayPanelController _mInstance;
        public static IEndOfDayPanelController Instance => _mInstance;
        #endregion
        public void Initialize(IRentValuesCatalogue injectionClass1, IPlayerGameProfile injectionClass2, IFoodValuesCatalogue injectionClass3, ITransportValuesCatalogue injectionClass4)
        {
            if (_mInitialized)
            {
                return;
            }
            _mRentCatalogue = injectionClass1;
            _currentPlayer = injectionClass2;
            _mFoodCatalogue = injectionClass3;
            _mTransportCatalogue = injectionClass4;

            UpdateDropdownValues();
            SetDayForDisplay(_currentPlayer.GetProfileCalendar().GetCurrentWorkDayObject());
            OnRentValueChanged();
            OnFoodValueChanged();
            OnTransportValueChange();
            _mInitialized = true;
        }

        private void UpdateDropdownValues()
        {
            var transportsAvailable = _currentPlayer.GetLifestyleModule().GetUnlockedTransportList;
            var foodAvailable = _currentPlayer.GetLifestyleModule().GetUnlockedFoodList;
            transportOptDropdown.options.Clear();
            foodOptionDropdown.options.Clear();
            foreach (var transportDataObject in transportsAvailable)
            {
                var transportName = transportDataObject.GetTransportName;
                var optionData = new TMP_Dropdown.OptionData(transportName);
                transportOptDropdown.options.Add(optionData);
            }
            foreach (var foodDataObject in foodAvailable)
            {
                var foodName = foodDataObject.GetFoodName;
                var optionData = new TMP_Dropdown.OptionData(foodName);
                foodOptionDropdown.options.Add(optionData);
            }
        }
        private void Awake()
        {
            SetInitialParameters();
        }

        private void SetInitialParameters()
        {
            if (_mInstance != null && (EndOfDayPanelController) _mInstance != this)
            {
                Destroy(this);
            }
            _mInstance = this;
            ClearDisplayedValues();
            mContinueButton.onClick.RemoveAllListeners();
            mContinueButton.onClick.AddListener(GoToSecondPanel);
            mReturnButton.onClick.AddListener(ReturnToFirstPanel);
            _mInitialized = false;
        }
    
        private void ReturnToFirstPanel()
        {
            GameDirector.Instance.GetUIController.DeactivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.SECOND_PANEL);
            GameDirector.Instance.GetUIController.ActivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.FIRST_PANEL);
            mContinueButton.onClick.RemoveAllListeners();
            mContinueButton.onClick.AddListener(GoToSecondPanel);
        }
        private void GoToSecondPanel()
        {
            Debug.Log("GOING TO SECOND PANEL!");
            GameDirector.Instance.GetUIController.DeactivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.FIRST_PANEL);
            GameDirector.Instance.GetUIController.ActivateObject(CanvasBitId.EndOfDay, EndOfDayPanelsBitStates.SECOND_PANEL);
            mContinueButton.onClick.RemoveAllListeners();
            mContinueButton.onClick.AddListener(ConfirmAndFinishEndOfDay);
        }
        private void ConfirmAndFinishEndOfDay()
        {
            GameDirector.Instance.GetGeneralBackgroundFader.GeneralCurtainAppear();
            GameDirector.Instance.BeginNewDayProcess();
            SetInitialParameters();
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
            var currentRentType = _currentPlayer.GetLifestyleModule().GetCurrentPlayerRentType;
            var rentObject = _mRentCatalogue.GetRentObject(currentRentType);
            rentOmniCredits.text = rentObject.GetRentPrice.ToString();
            rentEnergy.text = rentObject.GetRentEnergyBonus.ToString();
            rentSanity.text = rentObject.GetRentSanityBonus.ToString();
        }
        private void DontPayRent()
        {
            //TODO: Accumulate Debt Process
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
                case FastFoodName:
                    SetFastFoodValues();
                    break;
                case SimpleCookName:
                    SetSimpleCookValues();
                    break;            
                case CoffeeSnacksName:
                    SetCoffeeSnacksValues();
                    break;
                case CompleteCookName:
                    SetCompleteCookValues();
                    break;
                case StreetFoodName:
                    SetStreetFoodValues();
                    break;
                case GourmetFoodName:
                    SetGourmetFoodValues();
                    break;
                default:
                    Debug.LogWarning("None of the Dropdown choices had one predetermined. MUST CHECK");
                    return;
            }
        }
        //TODO: This should be data downloaded from server. Create a ticket. 
        private void SteFoodValues(IFoodDataObject foodDataObject)
        {
            var transportCredits = foodDataObject.GetFoodPrice;
            var energy = foodDataObject.GetFoodEnergyBonus;
            var sanity = foodDataObject.GetFoodSanityBonus;
            foodOmniCredits.text = transportCredits.ToString();
            foodEnergy.text = energy.ToString();
            foodSanity.text = sanity.ToString();
        }
        private void SetFastFoodValues()
        {
        
            foodOmniCredits.text = "-1";
            foodEnergy.text = "+1";
            foodSanity.text = "-3";
        }
        private void SetSimpleCookValues()
        {
            foodOmniCredits.text = "-5";
            foodEnergy.text = "+2";
            foodSanity.text = "+4";
        }
        private void SetCoffeeSnacksValues()
        {
            foodOmniCredits.text = "-1";
            foodEnergy.text = "0";
            foodSanity.text = "+5";
        }
        private void SetCompleteCookValues()
        {
            foodOmniCredits.text = "-1";
            foodEnergy.text = "0";
            foodSanity.text = "+5";
        }
        private void SetStreetFoodValues()
        {
            foodOmniCredits.text = "-1";
            foodEnergy.text = "0";
            foodSanity.text = "+5";
        }
        private void SetGourmetFoodValues()
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
                case OmniBikeName:
                    var bikeObject = _mTransportCatalogue.GetTransportDataObject(TransportTypesId.OmniBike);
                    SetTransportValues(bikeObject);
                    break;
                case OmniCabName:
                    var cabObject = _mTransportCatalogue.GetTransportDataObject(TransportTypesId.OmniCab);
                    SetTransportValues(cabObject);
                    break;
                case OmniBusName:
                    var busObject = _mTransportCatalogue.GetTransportDataObject(TransportTypesId.OmniBus);
                    SetTransportValues(busObject);
                    break;   
                case OmniHuberName:
                    var huberObject = _mTransportCatalogue.GetTransportDataObject(TransportTypesId.OmniHuber);
                    SetTransportValues(huberObject);
                    break;
                case OmniDriverName:
                    var driverObject = _mTransportCatalogue.GetTransportDataObject(TransportTypesId.OmniDriver);
                    SetTransportValues(driverObject);
                    break;    
                case OmniCopterName:
                    var omniCopterObject = _mTransportCatalogue.GetTransportDataObject(TransportTypesId.OmniCopter);
                    SetTransportValues(omniCopterObject);
                    break;    
                case OmniWalkName:
                    var omniWalkObject = _mTransportCatalogue.GetTransportDataObject(TransportTypesId.Walk);
                    SetTransportValues(omniWalkObject);
                    break;    
                default:
                    return;
            }
        }
        //TODO: This should be data downloaded from server. Create a ticket. 
        private void SetTransportValues(ITransportDataObject transportDataObject)
        {
            var transportCredits = transportDataObject.GetTransportPrice;
            var energy = transportDataObject.GetTransportEnergyBonus;
            var sanity = transportDataObject.GetTransportSanityBonus;
            transportOmniCredits.text = transportCredits.ToString();
            transportEnergy.text = energy.ToString();
            transportSanity.text = sanity.ToString();
        }

        #endregion
    
        public void SetDayForDisplay(IWorkDayObject dayToDisplay)
        {
            _mDisplayedDay = dayToDisplay;
            UpdateUIObjectsWithDayData();
        }
        /// <summary>
        /// Only updates the first panel. Second panel is updated according to its own data
        /// </summary>
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
        
            endOfDayCreditsStart.text = "";
        }
        private void ClearDisplayedValues()
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
    }
}