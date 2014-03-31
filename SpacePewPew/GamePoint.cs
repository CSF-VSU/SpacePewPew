using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpacePewPew
{
    class GamePoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public GamePoint(float x, float y)
        {
            X = x; Y = y;
        }
    }
}
