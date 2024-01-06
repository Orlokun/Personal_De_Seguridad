using System.Collections.Generic;
using DataUnits.GameCatalogues;
using DataUnits.ItemSources;
using GamePlayManagement.ProfileDataModules;
using Unity.Profiling;

namespace GameDirection.TimeOfDayManagement
{
    public interface ILifestyleModule : IProfileModule
    {
        //Rent Variables
        public bool IsRentUnlocked(int playerXp, RentTypesId rentType);
        public bool IsRentActive(RentTypesId rentType);
        public IRentDataObject GetActiveRentType();
        RentTypesId GetCurrentPlayerRentType { get; }
        public void SetNewRentType(RentTypesId newRentType);
        
        //Food Variables
        public List<IFoodDataObject> GetUnlockedFoodList { get; } 
        public bool IsFoodUnlocked(int playerXp, FoodTypesId foodType);
        public void UpdateFoodUnlockedList();

        //Transport Variables
        public List<ITransportDataObject> GetUnlockedTransportList { get; }
    }
}