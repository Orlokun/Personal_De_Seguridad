using DialogueSystem.Interfaces;
using GamePlayManagement;
using Utils;

namespace DialogueSystem
{
    public interface IModularDialogueDataController : IInitialize
    {
        public IDialogueObject CreateInitialDayIntro(IPlayerGameProfile currentPlayer);
    }
}