using System.Drawing;

namespace SpacePewPew.UI
{
    public class LayoutManager
    {
        public LayoutManager(ScreenType screenType)
        {
            switch (screenType)
            {
                case ScreenType.GameMenu: 
                    break;
                default: break;
            }
        }

        public bool IsOverButton(Point x)
        {
            return false;
        }
    }
}
