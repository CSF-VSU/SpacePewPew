using System.Drawing;

namespace SpacePewPew.UI
{
    public class ListViewItem : IUiElement
    {
        public PointF Position { get; set; }

        public ListViewItem(PointF pos, string itemName, int shipCost)
        {
            Position = pos;
            ItemName = itemName;
            ShipCost = shipCost;
        }

        public string ItemName { get; set; }

        public int ShipCost { get; set; }

        public event EventHandler OnClick;

        public void InvokeOnClick()
        {
            OnClick();
        }

    }
}


