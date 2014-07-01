using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SpacePewPew.DataTypes;
using SpacePewPew.GameLogic;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.GameObjects.Ships.Abilities;
using SpacePewPew.GameObjects.Ships.Abilities.AbilityContainer;
using SpacePewPew.Players.Strategies;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    [Serializable]
    public class Map : IMapView, IMapAbilityView
    {
        #region Declarations
        private bool _activeSelectExists;
        private Dictionary<AbilityName, IAbility> _abilities;

        public bool IsResponding { get; set; }
        public Cell[,] MapCells { get; set; }
        public List<Point> Lightened { get; set; }
        public Point ChosenShip { get; set; }
        
        public Random Random { get; set; }

        
        #endregion

        #region Map Builders

        public Map()
        {
            Random = new Random();
            MapCells = new Cell[0, 0];

            _abilities = new Dictionary<AbilityName, IAbility> {{AbilityName.Heal, new Heal()}};

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
            MapCells[0, 0].Object = new Dock();
            MapCells[0, 1].Object = new Dock();
            MapCells[1, 0].Object = new Dock();
            MapCells[1, 1].Object = new Dock();
            MapCells[2, 0].Object = new Dock();
            MapCells[2, 1].Object = new Dock();

            MapCells[4,  2].Object = new Station();
            MapCells[13, 5].Object = new Station();
            MapCells[0,  6].Object = new Station();
            MapCells[17, 1].Object = new Station();

            MapCells[15, 6].Object = new Dock();
            MapCells[15, 7].Object = new Dock();
            MapCells[16, 6].Object = new Dock();
            MapCells[16, 7].Object = new Dock();
            MapCells[17, 6].Object = new Dock();
            MapCells[17, 7].Object = new Dock();
        }

        public void LoadFrom(IMapView map)
        {
            MapCells = map.MapCells;
        }

        #endregion

        #region Actions
        public Decision Click(Point p)
        {
            Unlight();
            if (_activeSelectExists)
            {
                if (IsEnemy(p) && CanReachForAttack(p))
                {
                    _activeSelectExists = false;
                    return Attack(p);
                }
                if (IsShipMine(p))
                {
                    if (MapCells[p.X, p.Y].Ship.TurnState != TurnState.Finished)
                    {
                        Lightened = Light(p);
                        ChosenShip = p;
                    }
                    return null;
                }
                if (CanReach(p) && (IsEmpty(p) || HasObstacle(p) && MapCells[p.X, p.Y].Object.IsPassable))
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
                    if (MapCells[p.X, p.Y].Ship.TurnState != TurnState.Finished)
                    {
                        Lightened = Light(p);
                        _activeSelectExists = true;
                        ChosenShip = p;
                    }
                }
            }
            return null;
        }

        private Decision Move(Point p)
        {
            var path = FindWay(ChosenShip, p);

            if (path == null)
                return null;

            //!!!
            MapCells[p.X, p.Y].Ship = MapCells[ChosenShip.X, ChosenShip.Y].Ship;
            MapCells[ChosenShip.X, ChosenShip.Y].Ship = null;
            if (MapCells[p.X, p.Y].Object is Station)
            {
                Game.Instance().StationCapture((MapCells[p.X, p.Y].Object as Station).OwnerColor);
                (MapCells[p.X, p.Y].Object as Station).Capture(Game.Instance().CurrentPlayer.Color);
            }
            //!!!

            MapCells[p.X, p.Y].Ship.RemainedSpeed -= path.Count;
            MapCells[p.X, p.Y].Ship.TurnState = TurnState.InAction;

            if (MapCells[p.X, p.Y].Ship.RemainedSpeed == 0)
            {
                MapCells[p.X, p.Y].Ship.TurnState = HasEnemyIn(GetShipsAround(p)) ? TurnState.InAction : TurnState.Finished;
            }

            return new Decision { DecisionType = DecisionType.Move, PointA = ChosenShip, PointB = p, Path = path, ShipIndex = -1 };
        }

        private Decision Attack(Point p)
        {
            var path = FindWay(ChosenShip, p);
            path.RemoveAt(0);

            var shipId = MapCells[p.X, p.Y].Ship.Id;

            Point nearEnemy;
            if (path.Count == 0)
            {
                nearEnemy = ChosenShip;
            }
            else
            {
                nearEnemy = path[0];

                //!!!
                MapCells[nearEnemy.X, nearEnemy.Y].Ship = MapCells[ChosenShip.X, ChosenShip.Y].Ship;
                MapCells[ChosenShip.X, ChosenShip.Y].Ship = null;
                if (MapCells[nearEnemy.X, nearEnemy.Y].Object is Station)
                {
                    Game.Instance().StationCapture((MapCells[nearEnemy.X, nearEnemy.Y].Object as Station).OwnerColor);
                    (MapCells[nearEnemy.X, nearEnemy.Y].Object as Station).Capture(Game.Instance().CurrentPlayer.Color);

                }
                //!!!
            }

            MapCells[nearEnemy.X, nearEnemy.Y].Ship.TurnState = TurnState.Finished;

            return new Decision { DecisionType = DecisionType.Attack, PointA = ChosenShip, PointB = p, Path = path, Battle = Battle(nearEnemy, p), ShipIndex = shipId };
        }

        private List<AttackInfo> Battle(Point self, Point enemy)
        {
            var ship = MapCells[self.X, self.Y].Ship;
            var enemyShip = MapCells[enemy.X, enemy.Y].Ship;

            var result = new List<AttackInfo>();

            var remainSelfAtks = ship.Volleys;
            var remainEnemyAtks = enemyShip.Volleys;

            var isSelfKilled = false;
            var isEnemyKilled = false;

            while (remainSelfAtks != 0 || remainEnemyAtks != 0)
            {
                if (remainSelfAtks > 0)
                {
                    isEnemyKilled = Shoot(ship, enemyShip, enemy, true, result);
                    remainSelfAtks--;
                }

                if (remainEnemyAtks > 0)
                {
                    isSelfKilled = Shoot(enemyShip, ship, self, false, result);
                    remainEnemyAtks--;
                }

                if (isEnemyKilled || isSelfKilled)
                    break;
            }

            return result;
        }

        private bool Shoot(Ship attacker, Ship target, Point coord, bool isAtkMine, List<AttackInfo> log)
        {
            var isDestroyed = false;
            var myDmg = Random.Next(attacker.MinDamage, attacker.MaxDamage);
            target.Health -= myDmg;
            if (target.Health <= 0)
            {
                MapCells[coord.X, coord.Y].Ship = null;
                isDestroyed = true;
            }
            var record = new AttackInfo {Damage = myDmg, IsDestroyed = isDestroyed, IsMineAttack = isAtkMine};
            log.Add(record);
            return isDestroyed;
        }

        #endregion

        #region Extra Methods

        public void PassTurnRefresh()
        {
            _activeSelectExists = false;
            ChosenShip = new Point();
            Lightened = new List<Point>();

            var color = Game.Instance().CurrentPlayer.Color;
            var ships = GetShipIterator(color);

            foreach (var ship in ships)
            {
                ship.RemainedSpeed = ship.Speed;
                ship.TurnState = TurnState.Ready;
            }
        }

        public bool IsBuildingArea(Point buildingCoordinate)
        {
            return MapCells[buildingCoordinate.X, buildingCoordinate.Y].Object is Dock;
        }

        private bool IsEnemy(Point p)
        {
            return HasShip(p) && (Game.Instance().CurrentPlayer.Color != MapCells[p.X, p.Y].Ship.Color);
        }

        private bool IsNeighbourCellEmpty(Cell c)
        {
            return (!(c.Visited) && (c.Object == null || c.Object.IsPassable) && c.Ship == null);
        }

        public void BuildShip(Ship ship, Point coord)
        {
            MapCells[coord.X, coord.Y].Ship = ship;
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
            reachable.Add(ChosenShip);
            return reachable.Select(FindNeighbours).Any(neighboursList => Enumerable.Contains(neighboursList, p));
        }

        public IEnumerable<Ship> GetShipsAround(Point p)
        {
            var neighbours = FindNeighbours(p);
            return from n in neighbours where MapCells[n.X, n.Y].Ship != null select MapCells[n.X, n.Y].Ship;
        }

        public bool HasEnemyIn(IEnumerable<Ship> ships)
        {
            return ships.Any(ship => ship.Color != Game.Instance().CurrentPlayer.Color);
        }


        private bool IsShipMine(Point p)
        {
            return HasShip(p) && (Game.Instance().CurrentPlayer.Color == MapCells[p.X, p.Y].Ship.Color);
        }

        public bool HasShip(Point p)
        {
            return MapCells[p.X, p.Y].Ship != null;
        }

        public Ship GetShipFromPoint(Point p)
        {
            return MapCells[p.X, p.Y].Ship;
        }

        public Point GetShipPosition(Ship s)
        {
            for (var j = 0; j < MapCells.GetLength(1); j++)
                for (var i = 0; i < MapCells.GetLength(0); i++)
                    if (MapCells[i,j].Ship == s)
                        return new Point(i,j);
            return new Point(-1, -1);
        }

        private bool HasObstacle(Point p)
        {
            return MapCells[p.X, p.Y].Object != null;
        }
        #endregion

        #region Ways and Areas

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
            var wayExists = false;
            var way = new List<Point>();
            int n = Consts.MAP_WIDTH, m = Consts.MAP_HEIGHT;
            for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                {
                    MapCells[i, j].Previous = new Point(0, 0);
                    MapCells[i, j].Visited = false;
                }
            int x = startPoint.X, y = startPoint.Y;
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
                    int oldX;
                    x = oldX = current.X;
                    int oldY;
                    y = oldY = current.Y;
                    var neighbours = FindNeighbours(current);
                    foreach (var t in neighbours)
                    {
                        x = t.X;
                        y = t.Y;

                        if (x == destination.X && y == destination.Y)
                        {
                            if (IsNeighbourCellEmpty(MapCells[t.X, t.Y]) || MapCells[x, y].Ship != null)
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

        private List<Point> Light(Point startPoint)
        {
            var speed = MapCells[startPoint.X, startPoint.Y].Ship.RemainedSpeed;
            if (speed < 1)
                return new List<Point>();
            var counter = 0;
            MapCells[startPoint.X, startPoint.Y].IsLightened = true;

            var lightened = new List<Point> { startPoint };

            LightNeighbours(ref lightened, startPoint);
            if (speed == 1)
                return lightened;
            counter++;
            for (int i = 0; i < lightened.Count; i++)
            {
                var tmp = lightened.ToList();
                foreach (var t in tmp)
                    LightNeighbours(ref lightened, t);
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

        private void Unlight()
        {
            for (var i = 0; i < Consts.MAP_WIDTH; i++)
                for (var j = 0; j < Consts.MAP_HEIGHT; j++)
                    MapCells[i, j].IsLightened = false;
        }

        #endregion

        #region Iterator
        public IEnumerable<Ship> GetShipIterator(PlayerColor color)
        {
            var result = new List<Ship>();
            for (var i = 0; i < MapCells.GetLength(0); i++)
                for (var j = 0; j < MapCells.GetLength(1); j++)
                    if (MapCells[i, j].Ship != null && MapCells[i, j].Ship.Color == color)
                        result.Add(MapCells[i, j].Ship);
            return result;
        }

        public IEnumerable<Ship> GetShipIterator()
        {
            for (var i = 0; i < MapCells.GetLength(0); i++)
                for (var j = 0; j < MapCells.GetLength(1); j++)
                    if (MapCells[i, j].Ship != null)
                        yield return MapCells[i, j].Ship;
        }

        /*public IEnumerable<Station> GetStationIterator(PlayerColor color)
        {
            for (var i = 0; i < MapCells.GetLength(0); i++)
                for (var j = 0; j < MapCells.GetLength(1); j++)
                    if (MapCells[i,j].Obstacle is Station && (MapCells[i,j].Obstacle as Station).OwnerColor == color)
                        yield return MapCells[i, j].Obstacle as Station;
        }*/

        #endregion

        #region Ship Abilities' Implementation

        public AbilityResult PerformAbility(AbilityName ability, Ship invokator)
        {
            var coords = GetShipCoordinates(invokator);
            return _abilities[ability].Perform(this, coords);
        }

        private Point GetShipCoordinates(Ship ship)
        {
            for (var j = 0; j < Consts.MAP_HEIGHT; j++)
            {
                for (var i = 0; i < Consts.MAP_WIDTH; i++)
                {
                    if (MapCells[i, j].Ship == ship)
                        return new Point(i, j);
                }
            }
            return new Point(-1, -1);
        }

        #endregion
    }
}
