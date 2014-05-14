using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SpacePewPew.GameLogic;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.Players.Strategies;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    [Serializable]
    public class Map : IMapView
    {
        #region Declarations
        private bool _activeSelectExists;

        public bool IsResponding { get; set; }
        public Cell[,] MapCells { get; set; }
        public List<Point> Lightened { get; set; }
        public Point ChosenShip { get; set; }

        #endregion
        
        public Map()
        {
            MapCells = new Cell[0, 0];
          
            _activeSelectExists = false;
            IsResponding = true;
        }

        public void CreateEmptyMap(int width, int height)
        {
            MapCells = new Cell[width, height];
            for (var i = 0; i < width; i++)
                for (var j = 0; j < height; j++)
                {
                    MapCells[i, j].IsLightened = false;
                }

            //CAREFULЪ HARDCODE DETEHTED!
            MapCells[2, 2].Obstacle = new Dock(); 
            MapCells[2, 3].Obstacle = new Dock();
            MapCells[3, 2].Obstacle = new Dock();
            MapCells[3, 3].Obstacle = new Dock();

            MapCells[2, 6].Obstacle = new Dock();
            MapCells[2, 7].Obstacle = new Dock();
            MapCells[3, 6].Obstacle = new Dock(); 
            MapCells[3, 7].Obstacle = new Dock();
        }

        public void LoadFrom(IMapView map)
        {
            MapCells = map.MapCells;
        }


        private Decision Move(Point p)
        {
            var path = FindWay(ChosenShip, p);

            MapCells[p.X, p.Y].Ship = MapCells[ChosenShip.X, ChosenShip.Y].Ship;
            MapCells[ChosenShip.X, ChosenShip.Y].Ship = null;

            return new Decision { DecisionType = DecisionType.Move, PointA = ChosenShip, PointB = p, Path = path, ShipIndex = -1 };
        }

        private Decision Attack(Point p)
        {
            var path = FindWay(ChosenShip, p);
            path.RemoveAt(0); //без последней клетки

            var shipId = MapCells[p.X, p.Y].Ship.Id;

            Point nearEnemy;
            if (path.Count == 0)
            {
                nearEnemy = ChosenShip;
            }
            else
            {
                nearEnemy = path[0];
                MapCells[nearEnemy.X, nearEnemy.Y].Ship = MapCells[ChosenShip.X, ChosenShip.Y].Ship;
                MapCells[ChosenShip.X, ChosenShip.Y].Ship = null;
            }
            //TODO : переписать атаку
            var damagePerTime = MapCells[nearEnemy.X, nearEnemy.Y].Ship.MaxDamage;
            var numberOfAttacks = MapCells[nearEnemy.X, nearEnemy.Y].Ship.Volleys;
            ReduceHealth(p, damagePerTime, numberOfAttacks);
            if (MapCells[p.X, p.Y].Ship.Health <= 0)
                MapCells[p.X, p.Y].Ship = null;

            return new Decision { DecisionType = DecisionType.Attack, PointA = ChosenShip, PointB = p, Path = path, ShipIndex = shipId };
        }

        private void ReduceHealth(Point enemy, int damage, int times)
        {
            MapCells[enemy.X, enemy.Y].Ship.Health -= damage * times;
        }

        private bool IsEnemy(Point p)
        {
            return HasShip(p) && (Game.Instance().CurrentPlayer.Color != MapCells[p.X, p.Y].Ship.Color);
        }

        private bool IsEmpty(Point p)
        {
            return !(HasObstacle(p) || HasShip(p));
        }

        private bool CanReach(Point p)
        {
            return Lightened.Contains(p);
        }

        private bool CanReachForAttack(Point p)
        {
            var reachable = Lightened.ToList();
            return reachable.Select(FindNeighbours).Any(neighboursList => Enumerable.Contains(neighboursList, p));
        }

        private void Unlight()
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                    MapCells[i, j].IsLightened = false;
        }

        //!!!
        public Decision Click(Point p)
        {
            Unlight();
            if (_activeSelectExists)
            {
                if (IsEnemy(p) && CanReachForAttack(p) && (!(p.Equals(ChosenShip))))
                {
                    _activeSelectExists = false;
                    return Attack(p);
                }
                if (IsShipMine(p))
                {
                    Lightened = Light(p);
                    ChosenShip = p;
                    return null;
                }
                if (CanReach(p) && (IsEmpty(p) || HasObstacle(p) && MapCells[p.X, p.Y].Obstacle.IsPassable))
                {
                    _activeSelectExists = false;
                    return Move(p);
                }
                ChosenShip = new Point();
                _activeSelectExists = false;
            }
            else
            {
                if (!HasShip(p))
                {
                    return null;
                }

                if (IsShipMine(p))
                {
                    Lightened = Light(p);
                    _activeSelectExists = true;
                    ChosenShip = p;
                }
            }
            return null;
        }


        private bool IsShipMine(Point p)
        {
            return HasShip(p) && (Game.Instance().CurrentPlayer.Color == MapCells[p.X, p.Y].Ship.Color);
        }

        private bool HasShip(Point p)
        {
            return MapCells[p.X, p.Y].Ship != null;
        }

        private bool HasObstacle(Point p)
        {
            return MapCells[p.X, p.Y].Obstacle != null;
        }

        private List<Point> FindNeighbours(Point c)
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
            bool wayExists = false;
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
                    var isDone = false;
                    wayExists = false;
                    var current = (Point)q.Dequeue();
                    x = oldX = current.X;
                    y = oldY = current.Y;
                    var neighbours = FindNeighbours(current);
                    foreach (var t in neighbours)
                    {
                        x = t.X;
                        y = t.Y;

                        if (x == destination.X && y == destination.Y)
                        {
                            if (IsNeighbourCellEmpty(MapCells[t.X, t.Y]) || MapCells[x, y].Ship != null) //TODO : check if enemy only
                            {
                                MapCells[t.X, t.Y].Visited = true;
                                MapCells[t.X, t.Y].Previous = new Point(oldX, oldY);
                                q.Enqueue(new Point(t.X, t.Y));
                                wayExists = true;
                                isDone = true;
                                break;
                            }
                        }
                        else
                        {
                            if (IsNeighbourCellEmpty(MapCells[t.X, t.Y]))
                            {
                                MapCells[t.X, t.Y].Visited = true;
                                MapCells[t.X, t.Y].Previous = new Point(oldX, oldY);
                                wayExists = true;
                                q.Enqueue(new Point(t.X, t.Y));
                            }
                        }

                    }

                    if (isDone)
                        break;
                }
            }

            var a = destination;
            if (!(wayExists))
                return null;
            while (!(a.Equals(startPoint)))
            {
                way.Add(new Point(a.X, a.Y));
                a = new Point(MapCells[a.X, a.Y].Previous.X, MapCells[a.X, a.Y].Previous.Y);
            }
            return way;
        }


        private bool IsNeighbourCellEmpty(Cell c)
        {
            return (!(c.Visited) && c.Obstacle == null && c.Ship == null);
        }


        private List<Point> Light(Point startPoint)
        {
            var speed = 5;
            var counter = 0;
            MapCells[startPoint.X, startPoint.Y].IsLightened = true;
            
            var lightened = new List<Point> { startPoint };
            
            LightNeighbours(ref lightened, startPoint);
            counter++;
            for (int i = 0; i < lightened.Count; i++)
            {
                var tmp = new List<Point>();
                for (int j = 0; j < lightened.Count; j++)
                    tmp.Add(lightened[j]);
                for (int j = 0; j < tmp.Count; j++)
                    LightNeighbours(ref lightened, tmp[j]);
                counter++;
                if (counter >= speed)
                    break;
            }
            return lightened;
        }

        private void LightNeighbours(ref List<Point> lightened, Point c)
        {            
            List<Point> neighbours = FindNeighbours(c);
            foreach (var t in neighbours)
            {
                MapCells[t.X, t.Y].IsLightened = true;
                if (!(lightened.Contains(t)))
                    lightened.Add(t);
            }
        } 

        #region Iterator
        public IEnumerable<Ship> GetShipIterator(PlayerColor color)
        {
            for (var i = 0; i < MapCells.GetLength(0); i++)
                for (var j = 0; j < MapCells.GetLength(1); j++)
                    if (MapCells[i, j].Ship != null && MapCells[i, j].Ship.Color == color)
                        yield return MapCells[i, j].Ship;
        }

        public IEnumerable<Station> GetStationIterator(PlayerColor color)
        {
            for (var i = 0; i < MapCells.GetLength(0); i++)
                for (var j = 0; j < MapCells.GetLength(1); j++)
                    if (MapCells[i,j].Obstacle is Station && (MapCells[i,j].Obstacle as Station).OwnerColor == color)
                        yield return MapCells[i, j].Obstacle as Station;
        }

        #endregion

        public void BuildShip(Ship ship, Point coord)
        {
            MapCells[coord.X, coord.Y].Ship = ship;
        }

        public void PassTurnRefresh()
        {
            _activeSelectExists = false;
            ChosenShip = new Point();
            Lightened = new List<Point>();
        }

        public bool IsBuildingArea(Point buildingCoordinate)
        {
            return MapCells[buildingCoordinate.X, buildingCoordinate.Y].Obstacle is Dock;
        }
    }
}
