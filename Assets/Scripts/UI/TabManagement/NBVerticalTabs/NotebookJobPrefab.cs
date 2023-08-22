using TMPro;
using UnityEngine;

namespace UI.TabManagement.NBVerticalTabs
{
    internal class NotebookJobPrefab : MonoBehaviour
    {
        [SerializeField] private TMP_Text mStoreName;
        [SerializeField] private TMP_Text mStorePhone;

        public void SetJobPrefabValues(string storeName, string storePhone)
        {
            mStoreName.text = storeName;
            mStorePhone.text = storePhone;
        }
    }
}