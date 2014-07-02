using System.Drawing;

namespace SpacePewPew
{
    public static class Consts
    {
        public static readonly int OGL_WIDTH  = 830;
        public static readonly int OGL_HEIGHT = 460;

        public static readonly int BUTTON_WIDTH = 20;
        public static readonly int BUTTON_HEIGHT = 6;

        public static readonly int CELL_SIDE = 6;

        public static readonly PointF MAP_START_POS = new PointF(10, 10);

        public static readonly int MAP_WIDTH = 18;
        public static readonly int MAP_HEIGHT = 8;

        public static readonly float SCREEN_HEIGHT = 100;
        public static readonly float SCREEN_WIDTH = SCREEN_HEIGHT * OGL_WIDTH/OGL_HEIGHT;
        public static readonly float STATUS_BAR_HEIGHT = 7;

        public static readonly float LISTVIEWITEM_WIDTH = 75;
        public static readonly float LISTVIEWITEM_HEIGHT = 2.5f * BUTTON_HEIGHT;
        
        public static readonly int INCOME_PER_STATION = 5;
        public static readonly int HEAL_POWER = 4;
        public static readonly int STATION_HEAL_POWER = 8;
        public static readonly int CORROSION_POWER = 7;
    }
}