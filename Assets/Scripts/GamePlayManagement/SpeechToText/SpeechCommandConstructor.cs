using System;

namespace GamePlayManagement.SpeechToText
{
    public class SpeechCommandConstructor : ISpeechCommandConstructor
    {
        public SpeechCommandConstructor()
        {
            
        }

        public ISpeechCommandObject BuildCommandObject(FirstUtteranceObject firstUtterance, SecondUtteranceObject secondUtterance,
            ThirdUtteranceObject thirdUtterance, FourthUtteranceObject fourthUtterance)
        {
            return new SpeechCommandObject(firstUtterance, secondUtterance, thirdUtterance, fourthUtterance);
        }
    }
    
    public class SpeechCommandObject : ISpeechCommandObject
    {
        public FirstUtteranceObject FirstUtterance { get; }
        public SecondUtteranceObject SecondUtterance { get; }
        public ThirdUtteranceObject ThirdUtterance { get; }
        public FourthUtteranceObject FourthUtterance { get; }

        public SpeechCommandObject(FirstUtteranceObject firstUtterance, SecondUtteranceObject secondUtterance,
            ThirdUtteranceObject thirdUtterance, FourthUtteranceObject fourthUtterance)
        {
            FirstUtterance = firstUtterance;
            SecondUtterance = secondUtterance;
            ThirdUtterance = thirdUtterance;
            FourthUtterance = fourthUtterance;
        }
    }

    public interface ISpeechCommandObject
    {
        public FirstUtteranceObject FirstUtterance { get; }
        public SecondUtteranceObject SecondUtterance { get; }
        public ThirdUtteranceObject ThirdUtterance { get; }
        public FourthUtteranceObject FourthUtterance { get; }
    }
}