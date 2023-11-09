namespace DataUnits
{
    public interface ICallableSupplier
    {
        public DialogueSpeakerId SpeakerIndex { get; set; }
        public void StartCalling(int playerLevel);
    }
}