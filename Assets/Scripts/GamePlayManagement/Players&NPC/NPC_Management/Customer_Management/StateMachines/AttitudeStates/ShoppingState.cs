using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.CustomerInterfaces;
using GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.MovementStates;

namespace GamePlayManagement.Players_NPC.NPC_Management.Customer_Management.StateMachines.AttitudeStates
{
    public class ShoppingState : IAttitudeState
    {
        private IBaseCustomer _mCharacter;

        public ShoppingState(BaseCharacterInScene character)
        {
            _mCharacter = (BaseCustomer)character;
        }

        public void Enter()
        {
            _mCharacter.WalkingDestinationReached += WalkingDestinationReached;
        }

        public void Exit()
        {
            _mCharacter.WalkingDestinationReached -= WalkingDestinationReached;
        }

        public void Update() { }
        public void WalkingDestinationReached()
        {
            _mCharacter.ChangeMovementState<IdleMovementState>();
            var pointOfInterest =  _mCharacter.GetPositionsManager.GetPoiData(_mCharacter.MCurrentPoiId);
            _mCharacter.SetTempProductOfInterest(pointOfInterest.ChooseRandomProduct());
            _mCharacter.ChangeAttitudeState<EvaluatingProductState>();
        }
    }
}