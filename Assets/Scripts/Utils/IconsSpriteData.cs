using UnityEngine;
namespace Utils
{
    public class IconsSpriteData
    {
        private const string BASE_RESOURCE_PATH = "UI/Inventory/ItemIconSprites/";
        public static Sprite GetSpriteForItemIcon(string spriteName)
        {
            var iconPath = BASE_RESOURCE_PATH + $"{spriteName}";
            var spriteObject = Resources.Load<Sprite>(iconPath);
            if (!spriteObject)
            {
                Debug.LogError($"[GetSpriteForItemIcon]Sprite named: {spriteName} must be found!");
            }
            return spriteObject;
        }
    }
}