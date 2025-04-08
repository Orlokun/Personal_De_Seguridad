using GameDirection;
using Utils;

namespace GamePlayManagement.SpeechToText
{
    public class SpeechToTextOperator : ISpeechToTextOperator, IInitializeWithArg1<IGameDirector>
    {
        private static ISpeechToTextOperator mInstance;
        public static ISpeechToTextOperator Instance => mInstance;
        public bool IsInitialized => _mInitialized;
        private bool _mInitialized;
        
        public SpeechToTextOperator()
        {
            if (mInstance == null)
            {
                mInstance = this;
            }
        }
        
        public void ReceiveTextFromVoice(string incomingText)
        {
            // Handle the incoming text from voice recognition
            
            
        }

        public void Initialize(IGameDirector injectionClass)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ISpeechToTextOperator
    {
    }

    public class PlayerVoiceOrder
    {
        
    }
}