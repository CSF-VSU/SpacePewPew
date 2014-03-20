using System.Collections.Generic;
using SpacePewPew.Ships;

namespace SpacePewPew
{
    public class GameState
    {
         
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
        }

        public static Game Instance()
        {
            return _instance ?? (_instance = new Game());
        }
        #endregion

        #region Declarations
        private Map _map;
        private List<Ship> _ships;
        #endregion
        
        #region
        private GameState GameState { get; set; }

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

    }
}
