using Utils;

namespace DialogueSystem.Interfaces
{
    public interface IDecisionObject
    {
        public int DecisionIndex { get; set; }
        public MyIntEvent MyIntEvent { get; }
    }
}