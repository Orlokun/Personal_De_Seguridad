using UnityEngine;

namespace DialogueSystem
{
    public static class RangeProcessor
    {
        public static int[] ProcessLinksStrings(string[] intRange)
        {
            var parsedRange = new int[intRange.Length];
            for (int i = 0; i < intRange.Length; i++)
            {
                var isParsed = int.TryParse(intRange[i], out var link);
                if (!isParsed)
                {
                    Debug.LogWarning("[ProcessLinksStrings] Links in Important dialogue objects must be a number");
                    continue;
                }
                parsedRange[i] = link;
            }
            return parsedRange;
        }
    }
}