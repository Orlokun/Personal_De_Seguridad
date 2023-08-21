using GamePlayManagement.BitDescriptions.Suppliers;
using UnityEngine;

namespace DataUnits.ItemScriptableObjects
{
    [CreateAssetMenu(menuName = "Items/TercerAireItemObject")]
    public class TercerAireItemObject : ItemObject, IItemManage
    {
        [SerializeField] protected TercerAireItemsBitId bitId;
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