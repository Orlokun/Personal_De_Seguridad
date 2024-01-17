using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomersInSceneManagerData
    {
        public JobSupplierBitId JobId { get; }
        public int[] InstantiationFrequencyRange { get; }
        public string ClientPrefabPaths { get; }
        public int GameDifficultyLvl { get; }
    }
}