using CameraManagement;
using DialogueSystem.Interfaces;
using GamePlayManagement;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace UI
{
    public interface IUIController
    {
        void DeactivateAllObjects();
        void StartMainMenuUI();
        event UIController.ReturnToBaseCanvasState OnResetCanvas;
        void ToggleDialogueObject(bool isActive);
        void ReturnToBaseGamePlayCanvasState();
        void InitializeBaseInfoCanvas(IPlayerGameProfile playerProfile);
        void UpdateInfoUI();
        void UpdateOfficeUIElement(int cameraState);
        void ActivateObject(CanvasBitId canvasBitId, int panel);
        void DeactivateObject(CanvasBitId canvasBitId, int panel);
        void ToggleBackground(bool toggleValue);
        IDialogueOperator DialogueOperator { get; }
        void HiredInJobFoundFeedbackEvent(JobSupplierBitId newJobSupplier);
        void ElementUnlockedFeedback(BitItemSupplier itemSupplier);
        void SyncUIStatusWithCameraState(GameCameraState currentCameraState, int indexCamera);
        bool IsObjectActive(CanvasBitId canvasBitId, int panel);
    }
}