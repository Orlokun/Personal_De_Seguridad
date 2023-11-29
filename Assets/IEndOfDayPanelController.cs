using DataUnits.GameCatalogues;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using Utils;

public interface IEndOfDayPanelController : IInitializeWithArg4<IRentValuesCatalogue, IPlayerGameProfile, IFoodValuesCatalogue, ITransportValuesCatalogue>
{
    bool IsInitialized { get; }
    void SetDayForDisplay(IWorkDayObject dayToDisplay);
}