using System.Drawing;
﻿using SpacePewPew.UI;

namespace SpacePewPew
{
    public class Drawer
    {
        public DrawDelegate drawButton;

        #region Singleton pattern
        private static Drawer _instance;

        protected Drawer(DrawDelegate DB)
        {
            drawButton = DB;
        }

        public static Drawer Instance(DrawDelegate DB)
        {
            return _instance ?? (_instance = new Drawer(DB));
        }
        #endregion

        public void Draw(GameState gameState, LayoutManager manager)
        {
            foreach (var button in manager.Buttons.Values)
            {
                DrawButton(button.Position);
            }
        }

        public void DrawButton(PointF pos)
        {
            drawButton(pos);
        }
    }
}
