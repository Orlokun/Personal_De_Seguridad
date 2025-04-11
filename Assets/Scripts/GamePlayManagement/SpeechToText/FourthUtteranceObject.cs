namespace GamePlayManagement.SpeechToText
{
    public struct FourthUtteranceObject
    {
        public FourthUtteranceTypes UtteranceType { get; }
        public string CodeWord { get; }
        public int NextIndex  { get; }
        public FourthUtteranceObject(FourthUtteranceTypes utteranceType, string codeWords, int index)
        {
            UtteranceType = utteranceType;
            CodeWord = codeWords;
            NextIndex = index;        }
    }
}