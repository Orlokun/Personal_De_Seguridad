using System.Collections.Generic;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.CharacterGenerator
{
    [System.Serializable]
    public class GeneratorExclude
    {
        public ExcludeItem ExcludeItem;
        public int targetIndex;
        public List<ExcludeIndexes> exclude = new List<ExcludeIndexes>();
    }
}