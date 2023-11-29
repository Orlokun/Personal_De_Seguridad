using System.Collections.Generic;
using DataUnits.ItemSources;

namespace DataUnits.GameCatalogues
{
    public interface ITransportValuesCatalogue
    {
        public List<ITransportDataObject> GetAllTransportDataObjects { get; }
        public ITransportDataObject GetTransportDataObject(TransportTypesId id);
    }
}