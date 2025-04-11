using System.Collections.Generic;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;

namespace GamePlayManagement.SpeechToText
{
    public interface ISpeechToTextData
    {
        public List<string> GetUniqueItemIds { get; }
        public List<string> GetUniqueClientId { get; }
        public List<string>  GetItemTypeCodes { get; }
        public List<string> GetItemNameCodes { get; }
        public List<string> GetItemOriginCodes { get; }
        public List<string> GetBaseClassCodes { get; }
        public List<string> GetActionCodes { get; }
    }
}