using System.Collections.Generic;
using System.Linq;
using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using GamePlayManagement;

namespace GameDirection.TimeOfDayManagement
{
    public class LifestyleModule : ILifestyleModule
    {
        #region Members
        private IPlayerGameProfile _activePlayer;
        private IRentValuesCatalogue _rentValuesCatalogue;
        private IFoodValuesCatalogue _foodValuesCatalogue;
        private ITransportValuesCatalogue _transportValuesCatalogue;
        
        //Active Usable stuff
        private RentTypesId _currentRentType;
        
        private List<IFoodDataObject> _mUnlockedFoodList = new List<IFoodDataObject>();
        private List<ITransportDataObject> _mUnlockedTransportsList = new List<ITransportDataObject>();
        #endregion
        
        public List<IFoodDataObject> GetUnlockedFoodList => _mUnlockedFoodList;
        public List<ITransportDataObject> GetUnlockedTransportList => _mUnlockedTransportsList;
        public LifestyleModule(IRentValuesCatalogue rentValuesCatalogue, IFoodValuesCatalogue foodValuesCatalogue,
            ITransportValuesCatalogue transportValuesCatalogue)
        {
            _rentValuesCatalogue = rentValuesCatalogue;
            _foodValuesCatalogue = foodValuesCatalogue;
            _transportValuesCatalogue = transportValuesCatalogue;
        }

        #region RentDataInterface
        public RentTypesId GetCurrentPlayerRentType => _currentRentType;
        public bool IsRentUnlocked(int playerXp, RentTypesId rentType)
        {
            return _rentValuesCatalogue.GetRentObject(rentType).GetUnlockLevel >= playerXp;
        }
        public bool IsRentActive(RentTypesId rentType)
        {
            return _currentRentType == rentType;
        }
        public IRentDataObject GetActiveRentType()
        {
            return _rentValuesCatalogue.GetRentObject(_currentRentType);
        }
        public void SetNewRentType(RentTypesId newRentType)
        {
            _currentRentType = newRentType;
        }
        #endregion

        #region FoodDataInterface
        public bool IsFoodUnlocked(int playerXp, FoodTypesId foodType)
        {
            return _foodValuesCatalogue.GetAllFoodDataObjects.Any(x =>
                x.FoodId == foodType && x.GetUnlockLevel <= playerXp);
        }
        public void UpdateFoodUnlockedList()
        {
            _mUnlockedFoodList = new List<IFoodDataObject>();

        }
        #endregion

        #region TransportDataInterface
        public bool IsTransportUnlocked(int playerXp, TransportTypesId transportType)
        {
            return _transportValuesCatalogue.GetAllTransportDataObjects.Any(x =>
                x.TransportId == transportType && x.GetUnlockLevel <= playerXp);
        }
        public List<ITransportDataObject> UnlockedTransportDataObjects => _mUnlockedTransportsList.FindAll
            (x=> x.GetUnlockLevel <= _activePlayer.GeneralOmniCredits);
        public void UpdateTransportUnlockedList()
        {
            _mUnlockedTransportsList = new List<ITransportDataObject>();
            _mUnlockedTransportsList = _transportValuesCatalogue.GetAllTransportDataObjects.FindAll(x =>
                x.GetUnlockLevel <= _activePlayer.GeneralOmniCredits);
        }
        #endregion
        
        public void SetProfile(IPlayerGameProfile currentPlayerProfile)
        {
            _activePlayer = currentPlayerProfile;
            _currentRentType = RentTypesId.MOM_DAD;
            UpdateFoodUnlockedList();
            UpdateTransportUnlockedList();
        }

        public void PlayerLostResetData()
        {
            UpdateFoodUnlockedList();
            UpdateTransportUnlockedList();
        }
    }
}