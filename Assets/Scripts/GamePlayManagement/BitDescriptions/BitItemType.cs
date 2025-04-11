namespace GamePlayManagement.BitDescriptions
{
    public enum BitItemType
    {
        GUARD_ITEM_TYPE = 1 << 0,
        CAMERA_ITEM_TYPE = 1 << 1,
        WEAPON_ITEM_TYPE = 1 << 2,
        TRAP_ITEM_TYPE = 1 << 3,
        SPECIAL_ITEM_TYPE = 1 << 4
    }
}