using System.Drawing;

namespace SpacePewPew.UI
{
    public class ListViewItem : IUiElement
    {
        public PointF Position { get; set; }

        public ListViewItem(PointF pos, string itemName)
        {
            Position = pos;
            ItemName = itemName;
        }

        public string ItemName { get; set; }

        public event EventHandler OnClick;

        public void InvokeOnClick()
        {
            OnClick();
        }

    }
}


