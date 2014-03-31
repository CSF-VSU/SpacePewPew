using System.Drawing;

namespace SpacePewPew
{
    public class Drawer
    {
        #region Singleton pattern
        private static Drawer _instance;

        protected Drawer(ButtonDrawDelegate DB)
        {
            drawButton = DB;
            DrawButton(new PointF(157, 90));
            DrawButton(new PointF(157, 50));
        }

        

        public static Drawer Instance(ButtonDrawDelegate DB)
        {
            return _instance ?? (_instance = new Drawer(DB));
        }
        #endregion

        public void Redraw()
        {
            //
        }

        public ButtonDrawDelegate drawButton;

        public void Draw(GameState gameState)
        {
            // do all drawing
        } 

        public void DrawButton(PointF pos)
        {
            drawButton(pos);
        }
    }
}
