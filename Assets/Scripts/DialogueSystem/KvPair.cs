using System;
namespace DialogueSystem
{
    [Serializable]
    public class KvPair<T1, T2>
    {
        public T1 Key;
        public T2 Value { get; set; }
    }
}