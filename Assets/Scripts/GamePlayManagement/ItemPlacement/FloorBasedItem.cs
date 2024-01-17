using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemManagement.Guards;
using UnityEngine;

namespace GamePlayManagement.ItemPlacement
{
    public class FloorBasedItem : BaseItemPlacement
    {
        //Prefabs that need to be instantiated
        public GameObject instantiatedPrefab;

        // Use this for initialization
        void Start () 
        {
            if (instantiatedPrefab == null)
            {
                return;
            }
            CreateAndDeactivatePrefab();
        }

        private void CreateAndDeactivatePrefab()
        {
            //Instantiate prefab
            MInstantiatedObject = Instantiate(instantiatedPrefab);
            MInstantiatedObject.SetActive(false);
            
            var objectData = (IBaseGuardGameObject)MInstantiatedObject.GetComponent<BaseGuardGameObject>();
            objectData.SetInPlacementStatus(true);
        }
    
        public override void OnItemClicked(IItemObject itemData)
        {
            base.OnItemClicked(itemData);
            FloorPlacementManager.Instance.AttachNewObject(itemData,MInstantiatedObject);    
            FloorPlacementManager.Instance.ToggleRoofObject(false);    
        }


    }
}