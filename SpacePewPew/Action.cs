using System.Collections.Generic;
using System.Drawing;

namespace SpacePewPew
{
    public class Action
    {
        public Action(Point startPos, Point destination)
        {
            StartPos = startPos;
            Destination = destination;
            IsReady = false;
        }

        public Action()
        {
            IsReady = false;
            StartPos = new Point(-1, -1);
            Destination = new Point(-1, -1); 
        }

        public void Refresh()
        {
            IsReady = false;
            StartPos = new Point(-1, -1);
            Destination = new Point(-1, -1); 
        }

       public int ShipId { get; set; }
       public Point StartPos { get; set; }
       public Point Destination { get; set; }
       public List<Point> Path { get; set; }
       public bool IsReady { get; set; }
    }
}
