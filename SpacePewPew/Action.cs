using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SpacePewPew
{
    public class Action
    {
        public Action(Point StartPos, Point Destination)
        {
            this.StartPos = StartPos;
            this.Destination = Destination;
            this.IsReady = false;
        }

        public Action()
        {
            this.IsReady = false;
            this.StartPos = new Point(-1, -1);
            this.Destination = new Point(-1, -1); 
        }

        public void Refresh()
        {
            this.IsReady = false;
            this.StartPos = new Point(-1, -1);
            this.Destination = new Point(-1, -1); 
        }

       public int ShipId { get; set; }
       public Point StartPos { get; set; }
       public Point Destination { get; set; }
       public List<Point> Path { get; set; }
       public bool IsReady { get; set; }
    }
}
