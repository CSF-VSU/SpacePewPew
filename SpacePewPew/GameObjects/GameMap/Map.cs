using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace SpacePewPew.GameObjects.GameMap
{
    public class Map : IMap
    {

        #region Declarations
        private bool _activeSelectExists;


        public bool IsResponding { get; set; }
        public Cell[,] MapCells { get; set; }
        public List<Point> Lightened { get; set; }
        public List<Point> Path { get; set; }
        public Point ChosenShip { get; set; }
        public SpacePewPew.FactoryMethod.BattleShipCreator Creator = new FactoryMethod.BattleShipCreator();

        #endregion
        

        public Map(int width, int height)
        {
            
            MapCells = new Cell[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                  //  MapCells[i, j].MapCo = new Point(i, j);
                    MapCells[i, j].IsLightened = false;
                }
          
            MapCells[2, 2].Ship = Creator.FactoryMethod();
            MapCells[2, 2].Ship.id = Creator.lastId;
            MapCells[3, 2].Ship = Creator.FactoryMethod();
            MapCells[3, 2].Ship.id = Creator.lastId;
            _activeSelectExists = false;
            Path = new List<Point>();
            IsResponding = true;
        }

        

        private void Move(Point p)
        {
            IsResponding = false;
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


        public void Click(Point p)
        {
            Unlight();
            if (_activeSelectExists)
            {
                if (IsEnemy(p) && IsClose(p))
                {
                    Attack();
                    _activeSelectExists = false;
                }
                else if (HasShip(p))
                {
                    Lightened = Light(p);
                }
                else if (CanReach(p) && (IsEmpty(p) || HasObstacle(p) && MapCells[p.X, p.Y].Obstacle.IsPassable))
                {
                    Move(p);
                    _activeSelectExists = false;
                }

                
            }
            else
            {
                if (!HasShip(p)) return;

                Lightened = Light(p);
                if (IsShipMine(p))
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


        private IEnumerable<Point> FindNeighbours(Point c)
        {
            var neighbours = new List<Point>();
            int x = c.X, y = c.Y, width = Consts.MAP_WIDTH, height = Consts.MAP_HEIGHT;
            if (x % 2 == 0)
            {
                if (y > 0)
                {
                    neighbours.Add(new Point(x, y - 1));
                    if (x > 0)
                        neighbours.Add(new Point(x - 1, y - 1));
                    if (x < width - 1)
                        neighbours.Add(new Point(x + 1, y - 1));
                }
                if (y < height - 1)
                    neighbours.Add(new Point(x, y + 1));
                if (x > 0)
                    neighbours.Add(new Point(x - 1, y));
                if (x < width - 1)
                    neighbours.Add(new Point(x + 1, y));
            }
            else
            {
                if (y > 0)
                    neighbours.Add(new Point(x, y - 1));
                if (x > 0)
                    neighbours.Add(new Point(x - 1, y));
                if (x < width - 1)
                    neighbours.Add(new Point(x + 1, y));
                if (y < height - 1)
                {
                    neighbours.Add(new Point(x, y + 1));
                    neighbours.Add(new Point(x - 1, y + 1));
                    if (x < width - 1)
                        neighbours.Add(new Point(x + 1, y + 1));
                }
            }
            return neighbours;
        }


        public List<Point> FindWay(Point startPoint, Point destination)
        {
            var way = new List<Point>();
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
            q.Enqueue(new Point(x, y));
            while (((x != destination.X) || (y != destination.Y)) && (q.Count > 0))
            {
                for (var i = 0; i < q.Count; i++) 
                {
                    Point current = (Point)q.Dequeue();
                    x = oldX = current.X;
                    y = oldY = current.Y;
                    var neighbours = FindNeighbours(current);
                    foreach (var t in neighbours)
                    {
                        x = t.X;
                        y = t.Y;
                        if (IsNeighbourCellEmpty(MapCells[t.X, t.Y]))
                        {
                            MapCells[t.X, t.Y].Visited = true;
                            MapCells[t.X, t.Y].Previous = new Point(oldX, oldY);
                            q.Enqueue(new Point(t.X, t.Y));
                        }
                    }
                }
            }

            var a = destination;
            while (!(a.Equals(startPoint)))
            {
                way.Add(new Point(a.X, a.Y));
                a = new Point(MapCells[a.X, a.Y].Previous.X, MapCells[a.X, a.Y].Previous.Y);
            }
            return way;
        }


        private bool IsNeighbourCellEmpty(Cell c)
        {
            return ((!(c.Visited)) && (c.Obstacle == null) && (c.Ship == null));
        }


        private List<Point> Light(Point startPoint)
        {
            var speed = 3; //!!!!!!!!!! HARD CODE //не учитывает скорость корабля!!1111111111
            var counter = 0;
            MapCells[startPoint.X, startPoint.Y].IsLightened = true;
            
            var lightened = new List<Point> { startPoint };

            foreach (var c in lightened)
            {
                LightNeighbours(ref lightened, c);
                counter++;
                if (counter >= speed)
                    break;
            }
            return lightened;
        }


        private void LightNeighbours(ref List<Point> lightened, Point c)
        {
            var neighbours = FindNeighbours(c);
            foreach (var t in neighbours)
            {
                MapCells[t.X, t.Y].IsLightened = true;
                if (!(lightened.Contains(t)))
                    lightened.Add(t);
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
