using GamePlayManagement.ItemManagement.Guards;
using UnityEngine;

namespace GamePlayManagement.ItemManagement
{
    [RequireComponent(typeof(BaseGuardGameObject))]
    public class GuardItemObject : BaseItemGameObject, IGuardItemObject
    {
        private IBaseGuardGameObject _guardGameObject;
        public IBaseGuardGameObject GuardData => _guardGameObject;
        
        private void Awake()
        {
            _guardGameObject = GetComponent<BaseGuardGameObject>();
            MItemId = _guardGameObject.CharacterId;
        }
        
        public override void SetInPlacementStatus(bool inPlacement)
        {
            _guardGameObject.SetInPlacementStatus(inPlacement);
        }
    }
}