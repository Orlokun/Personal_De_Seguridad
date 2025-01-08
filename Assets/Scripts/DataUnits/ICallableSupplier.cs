using DialogueSystem;

namespace DataUnits
{
    public interface ICallableSupplier
    {
        public string SpeakerName { get; }
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public void StartCalling(int playerLevel);
        public int StoreHighestUnlockedDialogue { get;}

    }
}