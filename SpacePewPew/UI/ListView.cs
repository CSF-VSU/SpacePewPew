using System.Drawing;

namespace SpacePewPew.UI
{
    public class ListView : IUiElement
    {
        public PointF Position { get; set; }
        public ListViewItem[] Items;
        public bool Visible { get; set; }
        public int Index { get; set; }

        public ListView(PointF pos)
        {
            Items = new ListViewItem[2];
            Position = pos;
            Visible = false;
            Index = 0;

            Items[0] = new ListViewItem(new PointF(Position.X + 1, Position.Y + 1), "Ship 0");
            Items[0].OnClick += () => { Index = 0; };
            
            Items[1] = new ListViewItem(new PointF(Position.X + 1, Position.Y + 1 + Consts.LISTVIEWITEM_HEIGHT), "Ship 1");
            Items[1].OnClick += () => { Index = 1; };
        }
    }
}
