using System.Drawing;
using System.Collections.Generic;

namespace SpacePewPew.UI
{
    public class LayoutManager
    {


        private Dictionary<string, GameButton> _buttons;
        public Dictionary<string, GameButton> Buttons
        {
            get 
            {
                return _buttons;
            }
            set
            {
                _buttons = value;
            }
        }

        public void AddListener(string id, EventHandler eh)
        {
            Buttons[id].OnClick += eh;
        }

        public LayoutManager(ScreenType screenType)
        {
            Buttons = new Dictionary<string, GameButton>();
            switch (screenType)
            {
                case ScreenType.GameMenu:
                    {
                        Buttons.Add("Exit", new GameButton(new PointF(157,90)));  //выход
                        AddListener("Exit", () => { MainForm.ActiveForm.Close(); });
                       

                        Buttons.Add("New Game", new GameButton(new PointF(157, 50)));
                        AddListener("New Game", () => { System.Windows.Forms.MessageBox.Show("Карта из шестигранников типа"); });
                        break;
                        //OglDrawer.DrawButton(new PointF(157, 94));
                    }
                default: break;
            }
        }

        public LayoutManager()
            : base()
        {
            Buttons = new Dictionary<string, GameButton>();
        }


        public bool OnClick(PointF pos)
        {
            bool result = false;
            foreach (var btn in Buttons)
            {
                if ((pos.X > btn.Value.Position.X) && (pos.X < btn.Value.Position.X + Consts.BUTTON_WIDTH) &&
                    (pos.Y > btn.Value.Position.Y) && (pos.Y < btn.Value.Position.Y + Consts.BUTTON_HEIGHT)) 
                {
                    btn.Value.InvokeOnClick();
                    result = true;
                    break;
                }
            }
            return result;
        }
        
    }
}
