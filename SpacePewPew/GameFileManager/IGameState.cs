using System;
using System.Collections.Generic;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.Players;

namespace SpacePewPew.GameFileManager
{
    [Serializable]
    public class GameState
    {
        public IMapView Map { get; set; }
        public List<Player> Players { get; set; }
        public int CurrentPlayer { get; set; }

        public GameState() {}

        public GameState(IMapView map, List<Player> players, int playerIndex)
        {
            Map = map;
            Players = players;
            CurrentPlayer = playerIndex;
        }
    }
}
