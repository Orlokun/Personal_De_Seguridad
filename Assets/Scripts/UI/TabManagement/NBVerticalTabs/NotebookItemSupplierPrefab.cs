using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabManagement.NBVerticalTabs
{
    internal class NotebookItemSupplierPrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text mStoreName;
        [SerializeField] private TMP_Text mStorePhone;
        [SerializeField] private Image mStoreLogo;

        public void SetSupplierPrefabValues(string storeName, string storePhone, Sprite storeLogo)
        {
            mStoreName.text = storeName;
            mStorePhone.text = storePhone;
            mStoreLogo.sprite = storeLogo;
        }
    }
}