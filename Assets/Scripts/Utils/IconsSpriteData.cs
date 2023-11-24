using UnityEngine;
namespace Utils
{
    public static class IconsSpriteData
    {
        private const string BASE_ITEMICONS_RESOURCE_PATH = "UI/Inventory/ItemIconSprites/";
        public static Sprite GetSpriteForItemIcon(string spriteName)
        {
            var iconPath = BASE_ITEMICONS_RESOURCE_PATH + $"{spriteName}";
            var spriteObject = Resources.Load<Sprite>(iconPath);
            if (!spriteObject)
            {
                Debug.LogError($"[GetSpriteForItemIcon]Sprite named: {spriteName} must be found!");
            }
            return spriteObject;
        }
        public static Sprite GetSpriteForJobSupplierIcon(string spriteName)
        {
            var iconPath = BASE_ITEMICONS_RESOURCE_PATH + $"{spriteName}";
            var spriteObject = Resources.Load<Sprite>(iconPath);
            if (!spriteObject)
            {
                Debug.LogError($"[GetSpriteForItemIcon]Sprite named: {spriteName} must be found!");
            }
            return spriteObject;
        }
    }
}