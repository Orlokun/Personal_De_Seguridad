using System;
using System.Collections.Generic;
using DataUnits.JobSources;
using DialogueSystem.Units;
using GamePlayManagement.BitDescriptions.Suppliers;

namespace DataUnits.GameCatalogues
{
    public interface IBaseJobsCatalogue
    {
        bool JobSupplierExists(JobSupplierBitId jobSupplier);
        IJobSupplierObject GetJobSupplierObject(JobSupplierBitId jobSupplier);
        List<IJobSupplierObject> JobSuppliersInData { get; }
        public Tuple<bool, int> JobSupplierPhoneNumberExists(int phoneDialed);
        public IJobSupplierObject GetJobSupplierSpeakData(DialogueSpeakerId speakerId);
    }
}