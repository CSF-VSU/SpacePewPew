using System.Drawing;
using System.Windows.Forms;
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
            public IObstacle Obstacle { get; set; }
        }

        private Cell[,] mapCells;

        public Map()
        {
            mapCells = new Cell[5,5];
            for (var i = 0; i < mapCells.GetLength(0); i++)
            {
                for (var j = 0; j < mapCells.GetLength(1); j++)
                {
                    mapCells[i, j].Coordinate = new Point(i, j);
                    mapCells[i, j].Obstacle = null;
                    mapCells[i, j].Ship = null;
                }
            }
        }

        /// <summary>
        /// Converts mouse pos into map coordinate
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Point ConvertPoint(PointF p)
        {
            return new Point();
        }

        public void Click(PointF p)
        {
            var pnt = ConvertPoint(p);
            CheckState(mapCells[pnt.X, pnt.Y]);
        }

        private void CheckState(Cell cell)
        {
            //
        }
    }
}
