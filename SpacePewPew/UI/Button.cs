using System.Drawing;

namespace SpacePewPew.UI
{
    class Button : IUiElement
    {
        public Point Position { get; set; }

        public bool Enabled { get; set; }
        public bool Transparent { get; set; }

        public void AddListener(/* some delegate to connect with */)
        {
            //
        }
    }
}
