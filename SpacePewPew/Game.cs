using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.FactoryMethod;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.Players;
using SpacePewPew.Players.Strategies;

namespace SpacePewPew
{
    public class GameState
    {
        public bool Paused { get; set; }
    }

    public class Memento
    {
        public Memento(GameState gameState)
        {
            State = gameState;
        }

        public GameState State { get; private set; }
    }

    public class Game
    {
        #region Singleton pattern

        private static Game _instance;
        public ScreenType GameScreen;

        protected Game()
        {
            _map = new Map(Consts.MAP_WIDTH, Consts.MAP_HEIGHT);

            _races = new Dictionary<RaceName, Race>();
            _races[RaceName.Human] = new Race(RaceName.Human);
            _races[RaceName.Swarm] = new Race(RaceName.Swarm);
            _races[RaceName.Dentelian] = new Race(RaceName.Dentelian);
            _races[RaceName.Kronolian] = new Race(RaceName.Kronolian);

            _players = new List<Player>(2) {new Player(PlayerColor.Red, true), 
                                            new Player(PlayerColor.Blue, true)};
            _currentPlayer = _players[0];
            _isResponding = true;

            GameScreen = ScreenType.MainMenu;
        }

        public static Game Instance()
        {
            return _instance ?? (_instance = new Game());
        }

        public void Init(Drawer drawer)
        {
            DecisionDone += drawer.DecisionHandler;
        }

        #endregion

        #region Declarations

        private bool _isResponding;
        public Map _map;
        private List<Player> _players;
        private Player _currentPlayer;
        private Dictionary<RaceName, Race> _races; 

        public event DecisionHandler DecisionDone;

        #endregion
        
        #region Memento
        public GameState GameState { get; set; }

        public Memento CreateMemento()
        {
            return new Memento(GameState);
        }

        public void LoadState(Memento memento)
        {
            GameState = memento.State;
        }
       
        private List<Memento> memento;

        public void SaveState()
        {
            memento.Add(CreateMemento());
        }
        #endregion

        #region Extra methods

        private Player PassTurn()
        {
            var index = _players.IndexOf(_currentPlayer);
            index++;
            if (index == _players.Count)
                index = 0;
            return _players[index];
        }

        #endregion

        #region Controlling

        public void Tick()
        {
            if (--_currentPlayer.TimeLeft == 0)
            {
                _currentPlayer = PassTurn();
            }

            var decision = _currentPlayer.Strategy.MakeDecision(_map);
            if (decision.DecisionType == DecisionType.Halt) return;
            
            DecisionDone.Invoke(this, new DecisionArgs { Decision = decision });
            _isResponding = false;
        }


        public void MouseClick(Point p)
        {
            if (_isResponding)
                _map.Click(p);
        }

        public void MouseMove(Point p)
        {
            //..
        }
        #endregion

        public IMap GetGameField()
        {
            return _map;
        }

        #region Facade

        public PlayerInfo GetPlayerInfo()
        {
            var result = new PlayerInfo {Color = _currentPlayer.Color};
            result.Ships = _map.GetShipIterator(result.Color).Count;
            result.Stations = _map.GeetStationIterator(result.Color).Count;
            result.Money = _currentPlayer.Money;
            return result;
        }

        #endregion
    }
}
