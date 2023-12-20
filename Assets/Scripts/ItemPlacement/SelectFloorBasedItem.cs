using UnityEngine;

namespace ItemPlacement
{
    public class SelectFloorBasedItem : BaseSelectItemForPlacement
    {
        //Prefabs that need to be instantiated
        public GameObject instantiatedPrefab;

        // Use this for initialization
        void Start () {
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

        }

        public override void OnItemClicked()
        {
            base.OnItemClicked();
            FloorPlacementManager.Instance.AttachNewObject(MInstantiatedObject);    
            FloorPlacementManager.Instance.ToggleRoofObject(false);    
        }
    }
}