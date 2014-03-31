using System.Drawing;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects
{
    public class Map
    {
        struct Cell
        {
            public Point Coordinate { get; set; }
            public Ship Ship { get; set; }
            public Obstacle Obstacle { get; set; }
        }

        private Cell[,] mapCells;


        /// <summary>
        /// Converts mouse pos into map coordinate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Point ConvertPoint(Point p)
        {
            return new Point();
        }

        public void Click(Point p)
        {
            //do stuff
        }
    }
}
