namespace DataUnits
{
    public interface ICallableSupplier
    {
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public void StartCalling(int playerLevel);
        public int StoreHighestUnlockedDialogue { get;}
        public int StoreHighestLockedDialogue { get;}

    }
}