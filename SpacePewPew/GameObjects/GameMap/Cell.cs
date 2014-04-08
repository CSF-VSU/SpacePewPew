using System.Drawing;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    public struct Cell
    {
        public Point Coordinate { get; set; }
        public Ship Ship { get; set; }
        public IObstacle Obstacle { get; set; }
        public Point MapCoords { get; set; }
    }
}
