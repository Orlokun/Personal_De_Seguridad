using System.Collections.Generic;
using DialogueSystem.Interfaces;

namespace DataUnits.JobSources
{
    public interface IJobSupplierDialogueModule
    {
        void LoadInitialDeflectionDialogues();
        void StartUnlockDialogueData();
        public Dictionary<int, ISupplierDialogueObject> DeflectionDialogues { get; }
        public Dictionary<int, ISupplierDialogueObject> ImportantDialogues { get; }
        public Dictionary<int, ISupplierDialogueObject> InsistenceDialogues { get; }
        public Dictionary<int, ISupplierDialogueObject> SupplierCallDialogues{ get; }
        public Dictionary<int, ISupplierCallDialogueDataObject> SupplierCallDialoguesDataDictionary { get; }
    }
}