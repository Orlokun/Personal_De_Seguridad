using System;
using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits.ItemScriptableObjects
{
    public interface IItemManage : IItemObject
    {
        public void SetBitId();
    }
    
    [CreateAssetMenu(menuName = "Items/MonchitoItemObject")]
    public class MonchitoItemObject : ItemObject, IItemManage
    {
        [SerializeField] protected MonchitoItemsBitId bitId;
        
        public void SetBitId()
        {
            if (bitId == 0)
            {
                return;
            }
            _bitId = (int)bitId;
        }
    }
}