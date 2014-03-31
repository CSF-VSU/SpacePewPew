using System.Drawing;

namespace SpacePewPew.GameObjects.MapObjects
{
    public abstract class Obstacle
    {
        public bool     IsPassable      { get; set; }
        public bool     IsDestructable  { get; set; }
        public Point    Coordinate      { get; set; }

    }
}
