using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SpacePewPew.DataTypes;
using SpacePewPew.GameFileManager;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.GameObjects.Ships;
using SpacePewPew.GameObjects.Ships.Abilities.AbilityContainer;
using SpacePewPew.Players;
using SpacePewPew.Players.Strategies;
using SpacePewPew.ShipBuilder;

namespace SpacePewPew.GameLogic
{
    [Serializable]
    public class Game
    {
        #region Singleton pattern

        private static Game _instance;

        protected Game()
        {
            Map = new Map();

            Races = new Dictionary<RaceName, Race>();
            Races[RaceName.Human] = new Race(RaceName.Human);
            Races[RaceName.Swarm] = new Race(RaceName.Swarm);
            Races[RaceName.Dentelian] = new Race(RaceName.Dentelian);
            Races[RaceName.Kronolian] = new Race(RaceName.Kronolian);
            
            IsResponding = true;

            ShipCreator.GetCreator();
        }

        public static Game Instance()
        {
            return _instance ?? (_instance = new Game());
        }

        public void Init(Drawer drawer)
        {
            Manager = new FileManager();
        }

        #endregion

        #region Declarations

        public bool IsResponding { get; set; }
        public Map Map { get; set; }
        
        public List<Player> Players { get; private set; }
        public Player CurrentPlayer { get; private set; }

        public Point BuildingCoordinate { get; set; }
        
        public Dictionary<RaceName, Race> Races { get; set; }

        public FileManager Manager { get; private set; }
        public bool TimerEnabled { get; set; }

        #endregion

        
        #region Extra methods

        public void PassTurn()
        {
            var index = Players.IndexOf(CurrentPlayer);
            index++;
            if (index == Players.Count)
                index = 0;
            Players[index].TimeLeft = Players[index].MaxTurnTime;
            CurrentPlayer = Players[index];

            CurrentPlayer.Money += CurrentPlayer.StationCount*Consts.INCOME_PER_STATION;

            Map.HealDockedShips();
            Map.PerformShipAbilities();
            Map.CalculateShipEffects();
            Map.PassTurnRefresh();

            Drawer.Instance().DrawAbilities();
        }

        private Player GetPlayerByColor(PlayerColor color)
        {
            return Players.FirstOrDefault(player => player.Color == color);
        }

        public void StationCapture(PlayerColor prevOwner)
        {
            GetPlayerByColor(CurrentPlayer.Color).StationCount++;

            if (prevOwner == PlayerColor.None) return;
            
            GetPlayerByColor(prevOwner).StationCount--;
        }

        public void BuildShip(int index)
        {
            if (!Map.IsBuildingArea(BuildingCoordinate)) return;
            var ship = Races[CurrentPlayer.Race].BuildShip(index);

            if (CurrentPlayer.Money < ship.Cost) return;
            CurrentPlayer.Money -= ship.Cost;
            ship.Color = CurrentPlayer.Color;
            Map.BuildShip(ship, BuildingCoordinate);
            //TODO: decisiontype.Buy может пригодиться, только если после покупки корабля, скажем, рисуется анимация его постройки/появления
        }

        public IMapView GetGameField()
        {
            return Map;
        }

        #endregion

        #region Controlling

        public void Tick()
        {
            if (!TimerEnabled) return;

            if (--CurrentPlayer.TimeLeft == 0)
            {
                PassTurn();
                return;
            }

            var decision = CurrentPlayer.Strategy.MakeDecision(Map);

            if (decision != null)
                Drawer.Instance().DrawClick(decision);
        }

        public void MouseClick(Point p)
        {
            if (!IsResponding) return;

            CurrentPlayer.Strategy.ClickAppeared = true;
            CurrentPlayer.Strategy.MousePos = p;
        }
        #endregion


        public void LoadGame(GameState state)
        {
            Players = state.Players;
            CurrentPlayer = Players[state.CurrentPlayer];
            Map.LoadFrom(state.Map);
        }


        public void StartNewGame()
        {
            Map.CreateEmptyMap(Consts.MAP_WIDTH, Consts.MAP_HEIGHT);

            Players = new List<Player>(2) {new Player(PlayerColor.Red, RaceName.Human, true), 
                                            new Player(PlayerColor.Blue, RaceName.Human, true)};
            CurrentPlayer = Players[0];
        }

        public bool CanBuildHere(Point cell)
        {
            return Map.IsBuildingArea(cell) && !Map.HasShip(cell);
        }
    }
}
