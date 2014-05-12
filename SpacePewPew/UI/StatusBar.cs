using System.Drawing;

namespace SpacePewPew.UI
{
    public class StatusBar : IUiElement
    {
        public StatusBar()
        {
            Position = new PointF(0, 0);
        }

        public string PlayerName { get; private set; }
        public byte StationCount { get; private set; }
        public PointF Position{ get; set; }
        public byte ResourcesGain { get; private set; }
        public int ResourcesCount { get; private set; }
    }
}
