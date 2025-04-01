namespace GamePlayManagement.LevelManagement.LevelObjectsManagement
{
    public interface IStoreProductObjectData
    {
        public ProductsLevelEden ProductId { get; }
        public string ProductName { get; }
        public int ProductType { get; }
        public int Quantity { get; }
        public int Price { get; }
        public int HideChances { get; }
        public int Tempting { get; }
        public int Punishment { get; }
        
        public string PrefabName { get; }
        public string ProductBrand { get; }
        public string ProductSpriteName { get; }
        public string ProductDescription { get; }
    }
}