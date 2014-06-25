using System.Drawing;
using Tao.OpenGl;

namespace SpacePewPew.UI
{
    public class GameTextBox : UiElement
    {
        public event EventHandler Tick;
        private int _width = 40;

        public string Text { get; set; }
        public bool HasFocus { get; set; }

        public GameTextBox(PointF position)
        {
            Position = position;
            Text = "";
            HasFocus = false;
        }

        public void TypeCharacter(char c)
        {
            Text += c;
        }

        public void EraseLastCharacter()
        {
            if (Text.Length == 0)
                return;

            Text = Text.Remove(Text.Length - 1);
        }

        public void Clear()
        {
            Text = "";
        }

        public override void Draw()
        {
            Gl.glColor3f(0.2f, 0.2f, 1);
            Rect(Position.X, Position.Y, Position.X + _width, Position.Y + 4);
            DrawString(new PointF(Position.X + 1, Position.Y + 3), Text);
        }

        public override bool Click(PointF pos)
        {
            return false;
        }
    }
}
