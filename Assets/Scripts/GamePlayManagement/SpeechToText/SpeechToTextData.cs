using System.Collections.Generic;
using System.Linq;
using DataUnits.ItemScriptableObjects;
using GamePlayManagement.BitDescriptions;

namespace GamePlayManagement.SpeechToText
{
    public class SpeechToTextData : ISpeechToTextData
    {
        public List<string> GetUniqueItemIds => _mUniqueItemId;
        public List<string> GetUniqueClientId => _mUniqueClientId;
        public List<string> GetItemTypeCodes => _mItemTypeCodes.Values.ToList();
        public List<string> GetItemNameCodes => _mItemNameCodes.Values.ToList();
        public List<string> GetItemOriginCodes => _mOriginCodes.Values.ToList();
        public List<string> GetBaseClassCodes => _mClassCodes.Values.ToList();
        
        
        public List<string> GetActionCodes => _mVoiceActioncodes.Values.ToList();
        
        
        
        private List<string> _mUniqueItemId = new List<string>(); 
        private List<string> _mUniqueClientId = new List<string>(); 
        
        private Dictionary<BitItemType, string> _mItemTypeCodes = new Dictionary<BitItemType, string>
        {
            {BitItemType.GUARD_ITEM_TYPE, "plastic cakes"},
            {BitItemType.CAMERA_ITEM_TYPE, "cameras"},
            {BitItemType.TRAP_ITEM_TYPE, "traps"},
            {BitItemType.SPECIAL_ITEM_TYPE, "special"},
        };
        
        private Dictionary<IItemObject, string> _mItemNameCodes = new Dictionary<IItemObject, string>();
        private Dictionary<ItemOrigin, string> _mOriginCodes = new Dictionary<ItemOrigin, string>
        {
            {ItemOrigin.Unknown, "flying chicken"},            
            {ItemOrigin.Earth, "primate doom"},
            {ItemOrigin.EarthPrime, "parking heaven"},
            {ItemOrigin.Uranus, "darkest itch"},
        };
        private Dictionary<ItemBaseRace, string> _mClassCodes = new Dictionary<ItemBaseRace, string>
        {
            {ItemBaseRace.Regular, "pasta"},
            {ItemBaseRace.Ancient, "coconut"},
            {ItemBaseRace.Robotic, "android muppet"},
            {ItemBaseRace.Magical, "ether"},
            {ItemBaseRace.Human, "jelly monster"},
            {ItemBaseRace.Undead, "kangaroo"},
        };


        private Dictionary<VoiceActions, string> _mVoiceActioncodes = new Dictionary<VoiceActions, string>
        {
            { VoiceActions.IgnoreClient, "pencil" },
            { VoiceActions.SmokeBreak, "electric santa" },
            { VoiceActions.KickOut, "fucking dork" },
            { VoiceActions.EscortOut, "dynamite" },
            { VoiceActions.Follow, "fork" },
            { VoiceActions.Attack, "advance" },
            { VoiceActions.Guard, "lamp" },
            { VoiceActions.Detain, "carpet" },
            { VoiceActions.ReturnPost, "crab" },
        };
    }

    public enum VoiceActions
    {
        IgnoreClient,
        SmokeBreak,
        KickOut,
        EscortOut,
        Follow,
        Attack,
        Guard,
        Detain,
        ReturnPost
    }
}