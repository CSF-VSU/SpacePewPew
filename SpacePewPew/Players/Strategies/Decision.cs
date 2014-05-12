using System;
using System.Drawing;
using System.Collections.Generic;

namespace SpacePewPew.Players.Strategies
{
    [Serializable]
    public class Decision
    {
        public DecisionType DecisionType { get; set; }
        public List<Point> Path { get; set; }
        public Point PointA { get; set; }  // A, B => Path
        public Point PointB { get; set; }
        public int ShipIndex { get; set; }
    }
}
