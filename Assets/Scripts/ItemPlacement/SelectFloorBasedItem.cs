using UnityEngine;
using UnityEngine.EventSystems;

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
            MInstantiatedObject=Instantiate(instantiatedPrefab);
            MInstantiatedObject.SetActive(false);
        }
    
    
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            FloorPlacementManager.Instance.AttachNewObject(MInstantiatedObject);    
        }
    
        //Check Mouse Down Event the mouse down interface
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            //Pass the object that needs to be instantiated to the manager
            FloorPlacementManager.Instance.AttachNewObject(MInstantiatedObject);
        }
    }
}