namespace UI
{
    public static class BasePanelsBitStates
    {
        public static int BASE_INFO = 1;
        public static int BIT_INFO_PANEL = 2;
        public static int PAUSE_MENU = 4;
    }
    public static class GameplayPanelsBitStates
    {
        public static int ITEM_SIDEBAR = 1;
        public static int ITEM_DETAILED_SIDEBAR = 2;
        public static int TEXT_BACKGROUND = 4;
        public static int BOTTOM_DIALOGUE = 8;
        public static int IN_GAME_CLIENTS_TALK = 16;
    }
    
    public enum OfficePanelsBitStates
    {
        NOTEBOOK = 1,
        PHONE = 2,
        RADIO_ASHTRAY = 4,
    }

    public static class MainMenuPanelsBitStates
    {
        public static int BG_PANEL = 1;
        public static int LOGO_IMAGE = 2;
        public static int MAIN_OPTIONS = 4;
    }
    
}