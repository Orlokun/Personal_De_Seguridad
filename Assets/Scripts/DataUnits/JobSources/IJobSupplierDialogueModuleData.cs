using System.Collections;
using System.Collections.Generic;
using DialogueSystem.Interfaces;

namespace DataUnits.JobSources
{
    public interface IJobSupplierDialogueModuleData
    {
        public Dictionary<int, ISupplierDialogueObject> DeflectionDialogues { get; }
        public Dictionary<int, ISupplierDialogueObject> ImportantDialogues { get; }
        public Dictionary<int, ISupplierDialogueObject> InsistenceDialogues { get; }
        public Dictionary<int, ISupplierDialogueObject> SupplierCallDialogues { get; }
        public void LoadInitialDeflectionDialogues();
        public Dictionary<int, ISupplierCallDialogueDataObject> SupplierCallDialoguesDataDictionary { get; }
        public IEnumerator DownloadDialogueData(DialogueType dialogueType, string url);
        public int NpcRequirementStatus { get; }

    }
}