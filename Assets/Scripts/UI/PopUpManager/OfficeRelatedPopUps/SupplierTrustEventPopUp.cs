using DataUnits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.PopUpManager.OfficeRelatedPopUps
{
    public class SupplierTrustEventPopUp : MonoBehaviour, ISupplierTrustEventPopUp
    {
        [SerializeField] private Image mSupplierImage;
        [SerializeField] private TMP_Text mSupplierName;
        
        [SerializeField] private Transform mTrustHolder;

        [SerializeField] private GameObject mTrustPrefab;
        [SerializeField] private GameObject mDistrustPrefab;

        public void ClearPreviousIcons()
        {
            foreach (Transform child in mTrustHolder)
            {
                Destroy(child.gameObject);
            }
        }
        public void StartTrustEventPopUp(ICallableSupplier supplier, int trustAmount)
        {
            var iconSprite = IconsSpriteData.GetSpriteForSupplierIcon(supplier.SpriteName);
            
            mSupplierImage.sprite = iconSprite;
            mSupplierName.text = supplier.SpeakerName;
            ProcessTrustAmount(trustAmount);
        }

        private void ProcessTrustAmount(int trustAmount)
        {
            if (trustAmount > 0)
            {
                ProcessPositiveTrust(trustAmount);
                return;
            }
            if(trustAmount < 0)
            {
                ProcessNegativeTrust(trustAmount);
            }
        }

        private void ProcessNegativeTrust(int trustAmount)
        {
            for(int i = 0; i> trustAmount; i--)
            {
                Instantiate(mDistrustPrefab, mTrustHolder);
            }
        }

        private void ProcessPositiveTrust(int trustAmount)
        {
            for(int i = 0; i< trustAmount; i++)
            {
                Instantiate(mTrustPrefab, mTrustHolder);
            }
        }
    }
}