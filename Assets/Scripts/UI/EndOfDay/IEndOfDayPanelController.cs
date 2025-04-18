using DataUnits.GameCatalogues;
using GameDirection.TimeOfDayManagement;
using GamePlayManagement;
using Utils;

namespace UI.EndOfDay
{
    public interface IEndOfDayPanelController : IInitializeWithArg4<IRentValuesCatalogue, IPlayerGameProfile, IFoodValuesCatalogue, ITransportValuesCatalogue>
    {
        bool MInitialized { get; }
        void SetDayForDisplay(IWorkDayObject dayToDisplay);
    }
}