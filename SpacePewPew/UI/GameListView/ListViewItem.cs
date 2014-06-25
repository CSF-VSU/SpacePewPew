using System.Drawing;
using SpacePewPew.DataTypes;
using Tao.OpenGl;

namespace SpacePewPew.UI.GameListView
{
    public class ListViewItem : UiElement
    {
        private uint _texNum;
        public string Text { get; set; }
        public string Appendix { get; set; }
        public bool Enabled { get; set; }
        public bool IsSelected { get; set; }

        public ListViewItem(PointF pos, ListViewItemData data)
        {
            Position = pos;
            _texNum = data.GlyphNum;
            Text = data.Text;
            Appendix = data.Appendix;
            IsSelected = false;
        }

        public override void Draw()
        {
            if (Enabled)
                Gl.glColor3f(0, 0, 0);
            else
                Gl.glColor3f(0.3f, 0.3f, 0.3f);

            Rect(Position.X, Position.Y, Position.X + Consts.LISTVIEWITEM_WIDTH, Position.Y + Consts.LISTVIEWITEM_HEIGHT);
            if (IsSelected)
                Frame(Position.X, Position.Y, Position.X + Consts.LISTVIEWITEM_WIDTH, Position.Y + Consts.LISTVIEWITEM_HEIGHT);

            DrawString(new PointF(Position.X + 2, Position.Y + Consts.LISTVIEWITEM_HEIGHT / 2 - 0.5f), Text);
            DrawString(new PointF(Position.X + 67, Position.Y + Consts.LISTVIEWITEM_HEIGHT / 2 - 0.5f), Appendix);
        }

        public override bool Click(PointF pos)
        {
            return (pos.X >= Position.X && pos.X <= Position.X + Consts.LISTVIEWITEM_WIDTH &&
                   pos.Y >= Position.Y && pos.Y <= Position.Y + Consts.LISTVIEWITEM_HEIGHT) && Enabled;
        }

        public ListViewItemData GetData()
        {
            return new ListViewItemData
            {
                Text = Text,
                Appendix = Appendix
            };
        }

        public event EventHandler OnClick;

        public void InvokeOnClick()
        {
            OnClick();
        }
    }
}