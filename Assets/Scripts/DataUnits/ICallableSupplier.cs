using DialogueSystem;

namespace DataUnits
{
    public interface ICallableSupplier
    {
        public string SpeakerName { get; }
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public void ReceivePlayerCall(int playerLevel);
        public int StoreHighestUnlockedDialogue { get;}

    }
}