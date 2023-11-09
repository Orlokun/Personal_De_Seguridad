using System.Collections.Generic;

namespace DataUnits.GameCatalogues.JsonCatalogueLoaders
{
    [System.Serializable]
    public class CatalogueFromDataGeneric
    {
        public string range { get; set; }
        public string majorDimension { get; set; }
        public List<List<string>> values { get; set; }
        //public List<JobSupplierObject> values;
    }
}