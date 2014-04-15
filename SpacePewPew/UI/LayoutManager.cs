using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace SpacePewPew.UI
{
    public class LayoutManager
    {
        public Dictionary<string, GameButton> Buttons
        {
            get; set;
        }

        public ScreenType ScreenType { get; private set; }


        public LayoutManager(ScreenType screenType)
        {
            ScreenType = screenType;
            Buttons = new Dictionary<string, GameButton>();
            switch (screenType)
            {
                case ScreenType.MainMenu:
                    {
                        Buttons["Exit"] = new GameButton(new PointF(157,90));
                        Buttons["Exit"].OnClick += () => { MainForm.ActiveForm.Close(); };

                        Buttons["New Game"] = new GameButton(new PointF(157, 50));
                        Buttons["New Game"].OnClick += () => { ScreenType = ScreenType.Game; };
                        break;
                    }
            }
        }


        public bool OnClick(PointF pos)
        {
            var result = false;
            foreach (var btn in Buttons.Where(btn => (pos.X > btn.Value.Position.X) && (pos.X < btn.Value.Position.X + Consts.BUTTON_WIDTH) &&
                                                     (pos.Y > btn.Value.Position.Y) && (pos.Y < btn.Value.Position.Y + Consts.BUTTON_HEIGHT)))
            {
                btn.Value.InvokeOnClick();
                result = true;
                break;
            }
            return result;
        }
    }
}
