using System.Collections.Generic;
using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public interface IRentValuesCatalogue
    {
        public List<IRentDataObject> GetAllRentDataObjects { get; }
        public IRentDataObject GetRentObject(RentTypesId id);
    }
}