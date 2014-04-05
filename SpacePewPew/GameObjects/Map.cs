using System.Drawing;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.GameObjects.Ships;
using System;

namespace SpacePewPew.GameObjects
{
    public class Map
    {
        struct Cell
        {
            public Point Coordinate { get; set; }
            public Ship Ship { get; set; }
            public IObstacle Obstacle { get; set; }
            public Point MapCoords{ get; set; }
        }

        private Cell[,] mapCells;

        public PointF LightenedCell { get; private set; }

        public Map( int width, int height)
        {
            mapCells = new Cell[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
           //     {
             /*       if (i % 2 == 0)
                        mapCells[i, j].ScreenCoors = new PointF(Consts.MAP_START_POS.X + i / 2 * 3 * Consts.CELL_SIDE, Consts.MAP_START_POS.Y + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE);
                    else
                        mapCells[i, j].ScreenCoors = new PointF((Consts.MAP_START_POS.X + 1.5f + (i - 1) / 2 * 3) * Consts.CELL_SIDE, Consts.MAP_START_POS.Y + (float)Math.Sqrt(3) * Consts.CELL_SIDE / 2 + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE);
                   */ mapCells[i, j].MapCoords = new Point(i, j);
               // }
        }

        public Point ConvertPoint(PointF p)
        {
            PointF center;
           // foreach (Cell a in mapCells)
            for (int i = 0; i < Consts.MAP_WIDTH; i++)
                for (int j = 0; j < Consts.MAP_HEIGHT; j++)
                {
                    if (i % 2 == 0) 
                        center = new PointF(Consts.MAP_START_POS.X + i / 2 * 3 * Consts.CELL_SIDE     + Consts.CELL_SIDE, Consts.MAP_START_POS.Y + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE    + Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                    else
                        center = new PointF(Consts.MAP_START_POS.X + (1.5f + (i - 1) / 2 * 3) * Consts.CELL_SIDE + Consts.CELL_SIDE, Consts.MAP_START_POS.Y + (float)Math.Sqrt(3) * Consts.CELL_SIDE / 2 + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE + Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                    if (Math.Sqrt(Math.Pow(p.X - center.X, 2) + Math.Pow(p.Y - center.Y, 2)) < Consts.CELL_SIDE)
                    {
                        LightenedCell = new PointF(center.X - Consts.CELL_SIDE, center.Y - Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                        return new Point(mapCells[i, j].MapCoords.X, mapCells[i, j].MapCoords.Y);
                    }
                }
           
            return new Point();

        }

        public void Click(Point p)
        {
            //do stuff
        }
    }
}
