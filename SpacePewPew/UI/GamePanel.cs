using System.Drawing;
using Tao.OpenGl;

namespace SpacePewPew.UI
{
    public class GamePanel : UiElement
    {
        private readonly float _width;
        private readonly float _height;

        public GamePanel(float width, float height)
        {
            _width  = width;
            _height = height;
        }

        public override void Draw()
        {
            Gl.glColor3f(0.5f, 0.5f, 0.5f);
            Rect(Position.X, Position.Y, Position.X + _width, Position.Y + _height);
            Frame(Position.X, Position.Y, Position.X + _width, Position.Y + _height);
        }

        public override bool Click(PointF pos)
        {
            return false;
        }
    }
}
