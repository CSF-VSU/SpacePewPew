using System.Collections.Generic;
using SpacePewPew.Ships;

namespace SpacePewPew
{
    class Game
    {
        #region Singleton pattern
        private static Game _instance;

        protected Game()
        {
        }

        public static Game Instance()
        {
            // отложенная инициализация. Ништячки.
            return _instance ?? (_instance = new Game());
        }
        #endregion

        private Map _map;
        private List<Ship> _ships;
    }
}
