using System.Collections.Generic;
using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public interface IFoodValuesCatalogue
    {
        public List<IFoodDataObject> GetAllFoodDataObjects { get; }
        public IFoodDataObject GetFoodDataObject(FoodTypesId id);
    }
}