using System.Drawing;

namespace SpacePewPew.Players.Strategies
{
    public class Decision
    {
        public DecisionType DecisionType { get; set; }
        public Point PointA { get; set; }  // A, B => Path
        public Point PointB { get; set; }
        public int ShipIndex { get; set; }
    }
}
