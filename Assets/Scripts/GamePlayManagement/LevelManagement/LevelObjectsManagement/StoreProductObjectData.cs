namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public class StoreProductObjectData :  IStoreProductObjectData
    {
        protected ProductsLevelEden MProductId;
        protected string MProductName;
        protected int MProductType;
        protected int MQuantity;

        protected int MPrice;
        protected int MHideChances;
        protected int MTempting;
        protected int MPunishment;
        
        //PrefabManagement
        protected string MPrefabName;
        protected string MProductBrand;
        protected string MProductSpriteName;
        protected string MProductDescription;


        public StoreProductObjectData(ProductsLevelEden id, string name, int type, int quantity, int price, int hideChances, int tempting, int punishment,
            string prefabName, string productBrand, string productSpriteName,string productDescription)
        {
            MProductId = id;
            MProductName = name;

            MProductType = type;
            MQuantity = quantity;
            MPrice = price;
            MHideChances = hideChances;
            MTempting = tempting;
            MPunishment = punishment;
            
            MPrefabName = prefabName;
            MProductBrand = productBrand;
            MProductSpriteName = productSpriteName;
            MProductDescription = productDescription;
            
        }
        
        public ProductsLevelEden ProductId => MProductId;
        public string ProductName => MProductName;
        public int ProductType => MProductType;
        public int Quantity => MQuantity;

        public int Price => MPrice;
        public int HideChances => MHideChances;
        public int Tempting => MTempting;
        public int Punishment => MPunishment;
        
        public string PrefabName => MPrefabName;
        public string ProductBrand => MProductBrand;
        public string ProductSpriteName => MProductSpriteName;
        public string ProductDescription => MProductDescription;
    }
}