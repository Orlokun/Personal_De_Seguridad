using DialogueSystem;
using DialogueSystem.Units;
using GamePlayManagement;

namespace DataUnits
{
    public interface ICallableSupplier
    {
        public string SpeakerName { get; }
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public void ReceivePlayerCall(IPlayerGameProfile playerData);
        public int StoreHighestUnlockedDialogue { get;}

    }
}