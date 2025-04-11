namespace GamePlayManagement.SpeechToText
{
    internal interface ISpeechCommandConstructor
    {
        ISpeechCommandObject BuildCommandObject(FirstUtteranceObject firstUtterance, SecondUtteranceObject secondUtterance, ThirdUtteranceObject thirdUtterance, FourthUtteranceObject fourthUtterance);
    }
}