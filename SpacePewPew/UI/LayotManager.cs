using System.Collections;
using System.Drawing;

namespace SpacePewPew.UI
{
    class LayotManager
    {
        private Hashtable layout;

        public LayotManager()
        {
            layout["button1"] = new Button();
        }

        public bool IsOverButton(Point p)
        {
            return false;
        }
    }
}
