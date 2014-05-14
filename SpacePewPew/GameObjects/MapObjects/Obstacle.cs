using System;
using System.Drawing;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public abstract class IObstacle
    {
        public bool     IsPassable      { get; set; }
        public bool     IsDestructable  { get; set; }
        //public Point    Coordinate      { get; set; }
    }
}
