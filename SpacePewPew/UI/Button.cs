using System.Drawing;

namespace SpacePewPew.UI
{
    public delegate void EventHandler();

    public class GameButton : IUiElement
    {
        public PointF Position { get; set; }

        public GameButton(PointF pos, bool enabled)
        {
            Position = pos;
            Enabled = enabled;
        }

        public bool Enabled { get; set; }
        public bool Transparent { get; set; }
        
        public event EventHandler OnClick;

        public void InvokeOnClick()
        {
            OnClick();
        }
    }
}
