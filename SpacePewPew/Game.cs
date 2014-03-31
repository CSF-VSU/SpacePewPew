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

        protected Game()
        {
            _factory = new Factory();
            _map = new Map();
            _players = new List<Player>(1) {new Player(PlayerColor.Red)};
            _isResponding = true;
        }

        public static Game Instance()
        {
            return _instance ?? (_instance = new Game());
        }
        #endregion


        #region Declarations

        private bool _isResponding;
        private Factory _factory;
        private Map _map;
        private List<Player> _players;

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
