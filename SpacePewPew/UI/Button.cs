using System.Drawing;
using Tao.OpenGl;

namespace SpacePewPew.UI
{
    public delegate void EventHandler();

    public class GameButton : UiElement
    {
        public string Caption;
        public bool Enabled { get; set; }
        public bool Transparent { get; set; }

        public GameButton(PointF pos, string caption)
        {
            Position = pos;
            Caption = caption;
            Enabled = true;
        }

        public override void Draw()
        {
            Gl.glColor3f(0, 0, 0);
            Rect(Position.X, Position.Y, Position.X + Consts.BUTTON_WIDTH, Position.Y + Consts.BUTTON_HEIGHT);

            Gl.glColor3f(1, 1, 0.3f);
            Frame(Position.X, Position.Y, Position.X + Consts.BUTTON_WIDTH, Position.Y + Consts.BUTTON_HEIGHT);

            DrawString(new PointF(Position.X + 2, Position.Y + 4), Caption);

            if (!Enabled)
            {
                Gl.glColor4f(0.5f, 0.5f, 0.5f, 0.7f);

                Gl.glEnable(Gl.GL_BLEND);
                Rect(Position.X, Position.Y, Position.X + Consts.BUTTON_WIDTH, Position.Y + Consts.BUTTON_HEIGHT);
                Gl.glDisable(Gl.GL_BLEND);
            }

            DrawString(new PointF(Position.X + 2, Position.Y + 4), Caption);
        }

        public override bool Click(PointF pos)
        {
            if (!(pos.X >= Position.X) || !(pos.X <= Position.X + Consts.BUTTON_WIDTH) || !(pos.Y >= Position.Y) ||
                !(pos.Y <= Position.Y + Consts.BUTTON_HEIGHT)) return false;

            if (!Enabled)
                return false;

            OnClick();
            return true;
        }

        public event EventHandler OnClick;

        public void InvokeOnClick()
        {
            OnClick();
        }
    }
}
