namespace GamePlayManagement.SpeechToText
{
    public struct FirstUtteranceObject
    {
        public FirstUtteranceTypes UtteranceType { get; }
        public string CodeWord { get; }
        public int NextIndex  { get; }

        public FirstUtteranceObject(FirstUtteranceTypes utteranceType, string word, int i)
        {
            UtteranceType = utteranceType;
            CodeWord = word;
            NextIndex = i;
        }
    }
}