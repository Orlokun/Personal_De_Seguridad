using DataUnits.GameRequests;

namespace UI.PopUpManager.InfoPanelPopUp
{
    public interface IRequirementDetailPopUp
    {
        void SetAndDisplayRequirementInfo(IGameRequest newsObject);
    }
}