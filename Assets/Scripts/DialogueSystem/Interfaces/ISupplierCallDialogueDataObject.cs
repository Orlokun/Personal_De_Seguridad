using GameDirection.TimeOfDayManagement;

namespace DialogueSystem.Interfaces
{
    public interface ISupplierCallDialogueDataObject
    {
        public int CallDialogueIndex { get ; }
        public int JobDay { get ; }
        public int CallHour { get ; }
        public int CallMinute { get ; }
        public PartOfDay PartOfDay { get ; }
    }
}