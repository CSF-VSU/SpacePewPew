using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace SpacePewPew.GameObjects.GameMap
{
    public class Map : IMap
    {
        #region Declarations

        public Cell[,] MapCells { get; set; }
        private bool _activeSelectExists;
        private List<Cell> _lightened;

        public PointF LightenedCell { get; set; }

        #endregion
        

        public Map(int width, int height)
        {
            MapCells = new Cell[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    MapCells[i, j].MapCoords = new Point(i, j);
                    MapCells[i, j].IsLightened = false;
                }
            _activeSelectExists = false;
            LightenedCell = Consts.MAP_START_POS;
        }

        public Point ConvertPoint(PointF p)
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                {
                    PointF center;
                    if (i % 2 == 0)
                        center = new PointF(Consts.MAP_START_POS.X + i / 2 * 3 * Consts.CELL_SIDE + Consts.CELL_SIDE, Consts.MAP_START_POS.Y + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE + Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                    else
                        center = new PointF(Consts.MAP_START_POS.X + (1.5f + (i - 1) / 2 * 3) * Consts.CELL_SIDE + Consts.CELL_SIDE, Consts.MAP_START_POS.Y + (float)Math.Sqrt(3) * Consts.CELL_SIDE / 2 + j * (float)Math.Sqrt(3) * Consts.CELL_SIDE + Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                    if (Math.Sqrt(Math.Pow(p.X - center.X, 2) + Math.Pow(p.Y - center.Y, 2)) < Consts.CELL_SIDE)
                    {
                        LightenedCell = new PointF(center.X - Consts.CELL_SIDE, center.Y - Consts.CELL_SIDE * (float)Math.Sqrt(3) / 2);
                        return new Point(MapCells[i, j].MapCoords.X, MapCells[i, j].MapCoords.Y);
                    }
                }

            return new Point();
        }

        private void Move(Point p)
        {

        }

        private void Attack()
        {

        }

        private bool IsEnemy(Point p)
        {
            return true;
        }

        private bool IsClose(Point p)
        {
            return true;
        }


        private bool IsEmpty(Point p)
        {
            return !(HasObstacle(p) || HasShip(p));
        }

        private bool CanReach(Point p)
        {
            return MapCells[p.X, p.Y].IsLightened;
        }

        private void Unlight()
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                    MapCells[i, j].IsLightened = false;
        }


        public void Click(PointF p)
        {
            var t = ConvertPoint(p);
            Unlight();
            if (_activeSelectExists)
            {
                if (IsEnemy(t) && IsClose(t))
                {
                    Attack();
                }
                else if (HasShip(t))
                    _lightened = Light(t);
                else if ((CanReach(t)) && (IsEmpty(t) || (HasObstacle(t) && MapCells[t.X, t.Y].Obstacle.IsPassable)))
                    Move(t);

                _activeSelectExists = false;
            }
            else
            {
                if (!HasShip(t)) return;

                _lightened = Light(t);
                if (IsShipMine(t))
                    _activeSelectExists = true;
            }
        } 

        private bool IsShipMine(Point p)
        {
            return true;
        }

        private bool HasShip(Point p)
        {
            return MapCells[p.X, p.Y].Ship != null;
        }

        private bool HasObstacle(Point p)
        {
            return MapCells[p.X, p.Y].Obstacle != null;
        }


        private IEnumerable<Cell> FindNeighbours(Cell c)
        {
            var neighbours = new List<Cell>();
            int x = c.MapCoords.X, y = c.MapCoords.Y, width = Consts.MAP_WIDTH, height = Consts.OGL_HEIGHT;
            if (x % 2 == 0)
            {
                if (y > 0)
                {
                    neighbours.Add(MapCells[x, y - 1]);
                    if (x > 0)
                        neighbours.Add(MapCells[x - 1, y - 1]);
                    if (x < width - 1)
                        neighbours.Add(MapCells[x + 1, y - 1]);
                }
                if (y < height - 1)
                    neighbours.Add(MapCells[x, y + 1]);
                if (x > 0)
                    neighbours.Add(MapCells[x - 1, y]);
                if (x < width - 1)
                    neighbours.Add(MapCells[x + 1, y]);
            }
            else
            {
                if (y > 0)
                    neighbours.Add(MapCells[x, y - 1]);
                if (x > 0)
                    neighbours.Add(MapCells[x - 1, y]);
                if (x < width - 1)
                    neighbours.Add(MapCells[x + 1, y]);
                if (y < height - 1)
                {
                    neighbours.Add(MapCells[x, y + 1]);
                    neighbours.Add(MapCells[x - 1, y + 1]);
                    if (x < width - 1)
                        neighbours.Add(MapCells[x + 1, y + 1]);
                }
            }
            return neighbours;
        }


        private List<Cell> FindWay(Point startPoint, Point destination)
        {
            var way = new List<Cell>();
            int n = Consts.MAP_WIDTH, m = Consts.MAP_HEIGHT;
            for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                {
                    MapCells[i, j].Previous = new Point(0, 0);
                    MapCells[i, j].Visited = false;
                }
            int x = startPoint.X, y = startPoint.Y;
            int oldX = x, oldY = y;
            MapCells[x, y].Visited = true;
            var q = new Queue();
            q.Enqueue(MapCells[x, y]);
            while (((x != destination.X) || (y != destination.Y)) && (q.Count > 0))
            {
                for (var i = 0; i < q.Count; i++) 
                {
                    var current = (Cell)q.Dequeue();
                    x = oldX = current.MapCoords.X;
                    y = oldY = current.MapCoords.Y;
                    var neighbours = FindNeighbours(current);
                    foreach (var t in neighbours)
                    {
                        x = t.MapCoords.X;
                        y = t.MapCoords.Y;
                        if (IsNeighbourCellEmpty(MapCells[t.MapCoords.X, t.MapCoords.Y]))
                        {
                            MapCells[t.MapCoords.X, t.MapCoords.Y].Visited = true;
                            MapCells[t.MapCoords.X, t.MapCoords.Y].Previous = new Point(oldX, oldY);
                            q.Enqueue(MapCells[t.MapCoords.X, t.MapCoords.Y]);
                        }
                    }
                }
            }
            var a = destination;
            while (!(a.Equals(startPoint)))
            {
                way.Add(MapCells[a.X, a.Y]);
                a = new Point(MapCells[a.X, a.Y].Previous.X, MapCells[a.X, a.Y].Previous.Y);
            }
            return way;
        }


        private bool IsNeighbourCellEmpty(Cell c)
        {
            return ((!(c.Visited)) && (c.Obstacle == null) && (c.Ship == null));
        }

        private List<Cell> Light(Point startPoint)
        {
            var speed = 3; //!!!!!!!!!! HARD CODE //не учитывает скорость корабля!!1111111111
            var counter = 0;
            MapCells[startPoint.X, startPoint.Y].IsLightened = true;
            var lightened = new List<Cell> {MapCells[startPoint.X, startPoint.Y]};
            foreach (var c in lightened)
            {   
                LightNeighbours(ref lightened, c);
                counter++;
                if (counter >= speed)
                    break;
            }
            return lightened;
        }

        private void LightNeighbours(ref List<Cell> lightened, Cell c)
        {
            var neighbours = FindNeighbours(c);
            foreach (var t in neighbours)
            {
                MapCells[t.MapCoords.X, t.MapCoords.Y].IsLightened = true;
                if (!(lightened.Contains(MapCells[t.MapCoords.X, t.MapCoords.Y])))
                    lightened.Add(MapCells[t.MapCoords.X, t.MapCoords.Y]);
            }
        } 

        #region Iterator
        public ShipIterator GetShipIterator(PlayerColor color)
        {
            return new ShipIterator(MapCells, color);
        }

        public StationIterator GeetStationIterator(PlayerColor color)
        {
            return  new StationIterator(MapCells, color);
        }
        #endregion
    }
}
