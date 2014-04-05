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
        public static readonly int MAP_WIDTH = 4;
        public static readonly int MAP_HEIGHT = 4;

        public static readonly float RIGHT = 100 * (float)OGL_WIDTH/OGL_HEIGHT; 
    }
}
