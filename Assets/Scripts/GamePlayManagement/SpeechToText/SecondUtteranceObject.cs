namespace GamePlayManagement.SpeechToText
{
    public struct SecondUtteranceObject
    {
        public SecondUtteranceTypes UtteranceType { get; }
        public string CodeWord { get; }
        public int NextIndex  { get; }

        public SecondUtteranceObject(SecondUtteranceTypes utteranceType, string word, int i)
        {
            UtteranceType = utteranceType;
            CodeWord = word;
            NextIndex = i;
        }
    }
}