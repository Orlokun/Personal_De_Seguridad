using System.Collections.Generic;
using Utils;

namespace DialogueSystem.Interfaces
{
    public interface IDialogueDecision : IDialogueObject
    {
        public List<string> DecisionPossibilities { get; set; }
        public int DecisionIndex { get; set; }
        public MyIntEvent MyIntEvent { get; }
    }
}