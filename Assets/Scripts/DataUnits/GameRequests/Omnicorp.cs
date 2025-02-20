using DialogueSystem;
using DialogueSystem.Units;

namespace DataUnits.GameRequests
{
    public class Omnicorp : IOmnicorp
    {
        private IRequestModule _mRequestModule;
        public Omnicorp()
        {
            _mRequestModule = new RequestModule(DialogueSpeakerId.Omnicorp);
        }
    }
}