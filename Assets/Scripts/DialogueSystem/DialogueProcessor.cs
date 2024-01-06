using UnityEngine;

namespace DialogueSystem
{
    public static class DialogueProcessor
    {
        public static int[] ProcessLinksStrings(string[] links)
        {
            var parsedLinks = new int[links.Length];
            for (int i = 0; i < links.Length; i++)
            {
                var isParsed = int.TryParse(links[i], out var link);
                if (!isParsed)
                {
                    Debug.LogWarning("[ProcessLinksStrings] Links in Important dialogue objects must be a number");
                    continue;
                }
                parsedLinks[i] = link;
            }
            return parsedLinks;
        }
    }
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