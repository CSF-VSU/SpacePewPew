using System.Drawing;

namespace SpacePewPew.UI
{
    class StatusBar : IUiElement
    {
        public string PlayerName { get; private set; }
        public byte stationCount { get; private set; }
        public PointF Position{ get; set; }
        public byte resourcesGain { get; private set; }
        public int resourcesCount { get; private set; }
    }
}
