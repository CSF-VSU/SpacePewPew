using System;
using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.GameFileManager;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.Players;
using SpacePewPew.Players.Strategies;
using SpacePewPew.Prototype;

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

            _races = new Dictionary<RaceName, Race>();
            _races[RaceName.Human] = new Race(RaceName.Human);
            _races[RaceName.Swarm] = new Race(RaceName.Swarm);
            _races[RaceName.Dentelian] = new Race(RaceName.Dentelian);
            _races[RaceName.Kronolian] = new Race(RaceName.Kronolian);
            
            IsResponding = true;
            IsShowingModal = false;

            ShipCreator.GetCreator();
        }

        public static Game Instance()
        {
            return _instance ?? (_instance = new Game());
        }

        public void Init(Drawer drawer)
        {
            DecisionDone += drawer.DecisionHandler;
            Manager = new FileManager();
        }

        #endregion

        #region Declarations

        public bool IsResponding { get; set; }
        public bool IsShowingModal { get; set; }

        public Map Map { get; set; }
        
        public List<Player> Players { get; private set; }
        public Player CurrentPlayer { get; private set; }

        public Point BuildingCoordinate { get; set; }
        
        private readonly Dictionary<RaceName, Race> _races;

        public event DecisionHandler DecisionDone;

        public FileManager Manager { get; private set; }

        #endregion

        
        #region Extra methods

        private Player PassTurn()
        {
            var index = Players.IndexOf(CurrentPlayer);
            index++;
            if (index == Players.Count)
                index = 0;
            Players[index].TimeLeft = Players[index].MaxTurnTime;
            return Players[index];
        }

        public void BuildShip(int index)
        {
            var ship = _races[CurrentPlayer.Race].BuildShip(index);
            ship.Color = CurrentPlayer.Color;
            Map.BuildShip(ship, BuildingCoordinate);
        }

        public IMapView GetGameField()
        {
            return Map;
        }

        #endregion

        #region Controlling

        public void Tick()
        {
            if (--CurrentPlayer.TimeLeft == 0)
            {
                CurrentPlayer = PassTurn();
                return;
            }

            var decision = CurrentPlayer.Strategy.MakeDecision(Map);

            if (decision != null && !IsShowingModal)
                Drawer.Instance().DecisionHandler(this, new DecisionArgs { Decision = decision });
        }

        public void MouseClick(Point p)
        {
            if (IsResponding)
            {
                CurrentPlayer.Strategy.ClickAppeared = true;
                CurrentPlayer.Strategy.MousePos = p;
            }
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
    }
}
