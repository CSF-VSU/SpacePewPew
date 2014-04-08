using System.Drawing;

namespace SpacePewPew
{
    public static class Consts
    {
        public static readonly int OGL_WIDTH  = 1300;
        public static readonly int OGL_HEIGHT = 700;

        public static readonly int BUTTON_WIDTH = 20;
        public static readonly int BUTTON_HEIGHT = 6;

        public static readonly int CELL_SIDE = 4;

        public static readonly PointF MAP_START_POS = new PointF(0, 0);

        public static readonly int MAP_WIDTH = 30;
        public static readonly int MAP_HEIGHT = 15;

        public static readonly float SCREEN_HEIGHT = 100;
        public static readonly float SCREEN_WIDTH = SCREEN_HEIGHT * OGL_WIDTH/OGL_HEIGHT;
    }
}
