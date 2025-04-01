using System.Collections.Generic;
using DialogueSystem.Units;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueObjectBaseData
    {
        public List<IDialogueNode> DialogueNodes { get; set; }

        public int TimesActivatedCount { get; }
    }
}