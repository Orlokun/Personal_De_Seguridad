using System;

namespace ExternalAssets.AdvancedPeopleSystem2.Scripts.ScriptableObjects
{
    [System.Serializable]
    public class CharacterSelectedElements: ICloneable
    {
        public int Hair = -1;
        public int Beard = -1;
        public int Hat = -1;
        public int Shirt = -1;
        public int Pants = -1;
        public int Shoes = -1;
        public int Accessory = -1;
        public int Item1 = -1;

        public object Clone()
        {
            CharacterSelectedElements clone = new CharacterSelectedElements();
            clone.Hair = this.Hair;
            clone.Beard = this.Beard;
            clone.Hat = this.Hat;
            clone.Shirt = this.Shirt;
            clone.Pants = this.Pants;
            clone.Shoes = this.Shoes;
            clone.Accessory = this.Accessory;
            clone.Item1 = this.Item1;
            return clone;
        }

        public int GetSelectedIndex(CharacterElementType type)
        {
            switch (type)
            {
                case CharacterElementType.Hat:
                    return Hat;
                case CharacterElementType.Shirt:
                    return Shirt;
                case CharacterElementType.Pants:
                    return Pants;
                case CharacterElementType.Shoes:
                    return Shoes;
                case CharacterElementType.Accessory:
                    return Accessory;
                case CharacterElementType.Hair:
                    return Hair;
                case CharacterElementType.Beard:
                    return Beard;
                case CharacterElementType.Item1:
                    return Item1;
            }
            return -1;
        }
        public void SetSelectedIndex(CharacterElementType type, int newIndex)
        {
            switch (type)
            {
                case CharacterElementType.Hat:
                    Hat = newIndex;
                    break;
                case CharacterElementType.Shirt:
                    Shirt = newIndex;
                    break;
                case CharacterElementType.Pants:
                    Pants = newIndex;
                    break;
                case CharacterElementType.Shoes:
                    Shoes = newIndex;
                    break;
                case CharacterElementType.Accessory:
                    Accessory = newIndex;
                    break;
                case CharacterElementType.Hair:
                    Hair = newIndex;
                    break;
                case CharacterElementType.Beard:
                    Beard = newIndex;
                    break;
                case CharacterElementType.Item1:
                    Item1 = newIndex;
                    break;
            }
        }
    }
}