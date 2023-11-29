using GameDirection.TimeOfDayManagement;

public interface IEndOfDayPanelController
{
    bool IsInitialized { get; }
    void SetDayForDisplay(IWorkDayObject dayToDisplay);
}