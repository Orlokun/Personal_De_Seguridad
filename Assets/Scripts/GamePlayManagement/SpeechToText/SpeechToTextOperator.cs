using System;
using GameDirection;
using UnityEngine;
using Utils;

namespace GamePlayManagement.SpeechToText
{
    public class SpeechToTextOperator : ISpeechToTextOperator, IInitializeWithArg1<IGameDirector>
    {
        private static ISpeechToTextOperator mInstance;
        public static ISpeechToTextOperator Instance => mInstance;
        public bool IsInitialized => _mInitialized;
        private bool _mInitialized;

        private ISpeechToTextData _mData;
        
        public SpeechToTextOperator()
        {
            if (mInstance == null)
            {
                mInstance = this;
            }
            _mData = new SpeechToTextData();
        }
        
        public void ReceiveTextFromVoice(string incomingText)
        {
            // Split the incoming text from voice recognition
            // if less or equal to two words, return. 

            var words = incomingText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= 0)
            {
                return;
            }
            Debug.Log("[SpeechToTextOperator.ReceiveTextFromVoice] Evaluating Text first words.");
            var firstUtterance = EvaluateFirstUtterance(words[0], words[1]);
            if (firstUtterance.UtteranceType == FirstUtteranceTypes.None)
            {
                Debug.LogWarning("[ReceiveTextFromVoice] Not a valid First Utterance");
                return;
            }
            Debug.Log($"[ReceiveTextFromVoice] First Utterance is valid! = {firstUtterance.CodeWord}");

            var secondUtterance = EvaluateSecondUtterance(firstUtterance.NextIndex, words);
            if (secondUtterance.UtteranceType == SecondUtteranceTypes.None)
            {
                Debug.LogWarning("[ReceiveTextFromVoice] Not a valid Second Utterance");
                return;
            }
            Debug.Log($"[ReceiveTextFromVoice] Second Utterance is valid! = {secondUtterance.CodeWord}");
            var thirdUtterance = EvaluateThirdUtterance(secondUtterance.NextIndex, words);
            if (secondUtterance.UtteranceType == SecondUtteranceTypes.None)
            {
                Debug.LogWarning("[ReceiveTextFromVoice] Not a valid Third Utterance");
                return;
            }
            Debug.Log($"[ReceiveTextFromVoice] Third Utterance is valid! = {thirdUtterance.CodeWord}");
            Debug.Log($"[ReceiveTextFromVoice] All Utterances are valid! = {firstUtterance.CodeWord} - {secondUtterance.CodeWord} - {thirdUtterance.CodeWord}");
            
        }

        private ThirdUtteranceObject EvaluateThirdUtterance(int startIndex, string[] words)
        {
            var firstWord = words[startIndex];
            var firstTwoWordsJoined = firstWord + words[startIndex + 1];
            var firstTwoWordsSep = firstWord + " " + words[startIndex + 1];

            //Step1 Check if its action
            if (_mData.GetActionCodes.Contains(firstWord))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.Action, firstWord, startIndex + 1);
            }

            if (_mData.GetActionCodes.Contains(firstTwoWordsJoined))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.Action, firstTwoWordsJoined, startIndex + 2);
            }

            //Step 2. Check if its an Unique Client in-game
            if (_mData.GetUniqueClientId.Contains(firstWord))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.TargetUniqueId, firstWord, startIndex + 1);
            }

            if (_mData.GetUniqueClientId.Contains(firstWord))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.TargetUniqueId, firstTwoWordsJoined,
                    startIndex + 1);
            }

            //Step 3. Check if its an Item Origin
            if (_mData.GetItemOriginCodes.Contains(firstWord))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.TargetOrigin, firstWord, 1);
            }

            if (_mData.GetItemOriginCodes.Contains(firstTwoWordsSep))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.TargetOrigin, firstTwoWordsSep, 2);
            }
            
            if (_mData.GetBaseClassCodes.Contains(firstWord))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.TargetClass, firstWord, 1);
            }
            if (_mData.GetItemOriginCodes.Contains(firstTwoWordsSep))
            {
                return new ThirdUtteranceObject(ThirdUtteranceTypes.TargetClass, firstTwoWordsSep, 2);
            }
            return new ThirdUtteranceObject(ThirdUtteranceTypes.None, "",0);
        }

        private SecondUtteranceObject EvaluateSecondUtterance(int startIndex, string[] words)
        {
            var firstWord = words[startIndex];
            var firstTwoWords = firstWord + " " + words[startIndex + 1];

            if(_mData.GetActionCodes.Contains(firstWord))
            {
                return new SecondUtteranceObject(SecondUtteranceTypes.Action, firstWord, startIndex +1);
            }
            if (_mData.GetActionCodes.Contains(firstTwoWords))
            {
                return new SecondUtteranceObject(SecondUtteranceTypes.Action, firstTwoWords,startIndex +2);
            }
            
            if (IsNumber(firstWord) || firstWord == "all")
            {
                return new SecondUtteranceObject(SecondUtteranceTypes.Quantifier, firstTwoWords,startIndex +2);
            }
            return new SecondUtteranceObject(SecondUtteranceTypes.None, "", 0);
        }

        private bool IsNumber(string firstWord)
        {
            return int.TryParse(firstWord, out var result);
        }

        private FirstUtteranceObject EvaluateFirstUtterance(string firstWord, string secondWord)
        {
            var firstTwoWords = firstWord + " " + secondWord;
            
            //Step 1. Check if its a Unique Item Placed in-game
            if(_mData.GetUniqueItemIds.Contains(firstWord))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.UniqueItemId, firstWord, 1);
            }
            if (_mData.GetUniqueItemIds.Contains(firstTwoWords))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.UniqueItemId, firstTwoWords, 2);
            }

            //Step 2. Check if its an Item Type
            if (_mData.GetItemTypeCodes.Contains(firstWord))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemType, firstWord, 1);
            }
            if (_mData.GetItemTypeCodes.Contains(firstTwoWords))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemType, firstTwoWords, 2);
            }
            //Step 3. Check if its an Item Name
            if (_mData.GetItemNameCodes.Contains(firstWord))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemType, firstWord, 1);
            }
            if (_mData.GetItemNameCodes.Contains(firstTwoWords))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemType, firstTwoWords, 2);
            }
            
            //Step 4. Check if its an Item Origin
            if (_mData.GetItemOriginCodes.Contains(firstWord))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemOrigin, firstWord, 1);
            }
            if (_mData.GetItemOriginCodes.Contains(firstTwoWords))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemOrigin, firstTwoWords, 2);
            }
            
            //Step 5. Check if its an Item Base Class
            if (_mData.GetBaseClassCodes.Contains(firstWord))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemClass, firstWord, 1);
            }
            if (_mData.GetBaseClassCodes.Contains(firstTwoWords))
            {
                return new FirstUtteranceObject(FirstUtteranceTypes.ItemClass, firstTwoWords, 2);
            }
            
            return new FirstUtteranceObject(FirstUtteranceTypes.None, "", 0);
        }
        
        public void Initialize(IGameDirector injectionClass)
        {
            throw new System.NotImplementedException();
        }
    }
}