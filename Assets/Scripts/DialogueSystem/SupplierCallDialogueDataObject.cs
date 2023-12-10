using DialogueSystem.Interfaces;
using GameDirection.TimeOfDayManagement;

namespace DialogueSystem
{
    public class SupplierCallDialogueDataObject : ISupplierCallDialogueDataObject
    {
        private int _callDialogueIndex;
        private int _jobDay;
        private int _callHour;
        private int _callMinute;
        private PartOfDay _partOfDay;
        
        public int CallDialogueIndex => _callDialogueIndex;
        public int JobDay => _jobDay;
        public int CallHour => _callHour;
        public int CallMinute => _callMinute;
        public PartOfDay PartOfDay => _partOfDay;
        public SupplierCallDialogueDataObject(int callDialogueIndex, int jobDay, int callHour, int callMinute, PartOfDay partOfDay)
        {
            _callDialogueIndex = callDialogueIndex;
            _jobDay = jobDay;
            _callHour = callHour;
            _callMinute = callMinute;
            _partOfDay = partOfDay;
        }
    }
}