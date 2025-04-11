using DataUnits.ItemScriptableObjects;
using GamePlayManagement.ItemManagement.Guards;
using UnityEngine;

namespace GamePlayManagement.ItemManagement
{
    [RequireComponent(typeof(BaseGuardGameController))]
    public class GuardItemObject : BaseItemGameObject, IGuardItemObject
    {
        private IBaseGuardGameController _guardGameController;
        public IBaseGuardGameController GuardData => _guardGameController;
        
        private void Awake()
        {
            _guardGameController = GetComponent<BaseGuardGameController>();
            MItemId = _guardGameController.CharacterId;
        }
        
        public override void SetInPlacementStatus(bool inPlacement)
        {
            _guardGameController.SetInPlacementStatus(inPlacement);
        }

        public override void InitializeItem(IItemObject itemData)
        {
            base.InitializeItem(itemData);
            _guardGameController.Initialize(itemData);
            _guardGameController.StartBehaviorTree();
        }
    }
}