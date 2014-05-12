using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.FactoryMethod;
using SpacePewPew.GameFileManager;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.Players;
using SpacePewPew.Players.Strategies;

namespace SpacePewPew
{
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

        public Point BuildingCoordinate { get; set; }

        
        private Player _currentPlayer;
        private readonly Dictionary<RaceName, Race> _races;

        public event DecisionHandler DecisionDone;

        public FileManager Manager { get; private set; }

        #endregion
        
        #region Extra methods

        private Player PassTurn()
        {
            var index = Players.IndexOf(_currentPlayer);
            index++;
            if (index == Players.Count)
                index = 0;
            return Players[index];
        }

        public void BuildShip(int index)
        {
            var ship = _races[_currentPlayer.Race].Builder[index].FactoryMethod();
            ship.Color = _currentPlayer.Color;
            Map.BuildShip(ship, BuildingCoordinate);
        }

        public IMap GetGameField()
        {
            return Map;
        }

        #endregion

        #region Controlling

        public void Tick()
        {
            if (--_currentPlayer.TimeLeft == 0)
            {
                _currentPlayer = PassTurn();
                return;
            }

            var decision = _currentPlayer.Strategy.MakeDecision(Map);

            if (decision != null && !IsShowingModal)
                Drawer.Instance().DecisionHandler(this, new DecisionArgs { Decision = decision });
        }

        public void MouseClick(Point p)
        {
            if (IsResponding)
            {
                _currentPlayer.Strategy.ClickAppeared = true;
                _currentPlayer.Strategy.MousePos = p;
            }
        }
        #endregion

        
        #region Facade

        public PlayerInfo GetPlayerInfo()
        {
            var result = new PlayerInfo {Color = _currentPlayer.Color};
            result.Ships = Map.GetShipIterator(result.Color).Count;
            result.Stations = Map.GeetStationIterator(result.Color).Count;
            result.Money = _currentPlayer.Money;
            result.TimeLeft = _currentPlayer.TimeLeft;
            return result;
        }

        #endregion

        public void LoadGame(GameState state)
        {
            Players = state.Players;
            //TODO: добавить в файл сохранения информацию о том, который из игроков ходит.
            _currentPlayer = Players[0];
            Map.LoadFrom(state.Map);
        }

        public void StartNewGame()
        {
            Map.CreateEmptyMap(Consts.MAP_WIDTH, Consts.MAP_HEIGHT);

            Players = new List<Player>(2) {new Player(PlayerColor.Red, RaceName.Human, true), 
                                            new Player(PlayerColor.Blue, RaceName.Human, true)};
            _currentPlayer = Players[0];
        }
    }
}
