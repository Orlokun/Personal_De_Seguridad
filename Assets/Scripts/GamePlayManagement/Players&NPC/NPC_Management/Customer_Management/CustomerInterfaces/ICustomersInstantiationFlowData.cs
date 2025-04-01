using System.Collections.Generic;
using GameDirection.GeneralLevelManager.ShopPositions;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces
{
    public interface ICustomersInstantiationFlowData
    {
        public JobSupplierBitId JobId { get; }
        public int[] InstantiationFrequencyRange { get; }
        public string ClientPrefabPaths { get; }
        public int GameDifficultyLvl { get; }
        public void SetEntrancePositions(List<IStoreEntrancePosition> storeEntrancePositions);
        public List <IStoreEntrancePosition> GetEntrancePositions { get; }
    }
}