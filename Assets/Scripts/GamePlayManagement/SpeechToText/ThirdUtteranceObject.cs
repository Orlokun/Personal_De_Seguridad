namespace GamePlayManagement.SpeechToText
{
    public struct ThirdUtteranceObject
    {
        public ThirdUtteranceTypes UtteranceType { get; }
        public string CodeWord { get; }
        public int NextIndex  { get; }

        public ThirdUtteranceObject(ThirdUtteranceTypes utteranceType, string word, int i)
        {
            UtteranceType = utteranceType;
            CodeWord = word;
            NextIndex = i;
        }
    }
}