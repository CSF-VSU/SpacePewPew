using System.Drawing;

namespace SpacePewPew.UI
{
    public class GameCheckBox : UiElement
    {
        public string Caption { get; set; }
        public bool Checked { get; set; }

        public GameCheckBox(PointF position)
        {
            Position = position;
            Checked = false;
        }

        public override void Draw()
        {
            var index = Checked ? 10u : 11u;
            var coords = new[]
            {
                new PointF(Position.X,     Position.Y),
                new PointF(Position.X + 5, Position.Y),
                new PointF(Position.X + 5, Position.Y + 5),
                new PointF(Position.X,     Position.Y + 5)
            };
            Drawer.Instance().DrawTexture(index, coords);

            DrawString(new PointF(Position.X + 6, Position.Y + 4), Caption);
        }

        public override bool Click(PointF pos)
        {
            if (!(pos.X >= Position.X) || !(pos.X <= Position.X + 5) || !(pos.Y >= Position.Y) ||
                !(pos.Y <= Position.Y + 5)) return false;

            Checked = !Checked;
            return true;
        }
    }
}
