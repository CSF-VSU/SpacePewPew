using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.FactoryMethod;
using SpacePewPew.GameObjects;
using SpacePewPew.Players;

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

            _players = new List<Player>(1) {new Player(PlayerColor.Red, true)};
            _isResponding = true;

            GameScreen = ScreenType.MainMenu;
        }

        public static Game Instance()
        {
            return _instance ?? (_instance = new Game());
        }
        #endregion

        #region Declarations

        private bool _isResponding;
        public Map _map;
        private List<Player> _players;
        private Dictionary<RaceName, Race> _races; 

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

        #region Controlling

        public void MouseClick(Point p)
        {
            _map.Click(p);
        }

        public void MouseMove(Point p)
        {
            //..
        }
        #endregion
    }
}
