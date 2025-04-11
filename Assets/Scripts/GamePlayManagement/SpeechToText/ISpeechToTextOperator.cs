namespace GamePlayManagement.SpeechToText
{
    public interface ISpeechToTextOperator
    {
        void ReceiveTextFromVoice(string resultText);
    }
}