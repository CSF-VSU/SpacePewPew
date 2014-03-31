using System.Drawing;

namespace SpacePewPew.UI
{
    public delegate void EventHandler();

    public class GameButton : IUiElement
    {
        // string caption; etc.
       
        public PointF Position { get; set; }
        public GameButton(PointF pos)
        {
            Position = pos;
        }
        public bool Enabled { get; set; }
        public bool Transparent { get; set; }
        
        public event EventHandler OnClick;

        public void AddListener(/* some delegate to connect with */)
        {
            
        }

        public void InvokeOnClick()
        {
            OnClick.Invoke();
        }
    }
}
