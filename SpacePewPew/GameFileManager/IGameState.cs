using System;
using System.Collections.Generic;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.Players;

namespace SpacePewPew.GameFileManager
{
    //public interface IGameState
    
    [Serializable]
    public class GameState
    {
        //[XmlAttribute]
        public IMap Map { get; set; }
        
        //[XmlAttribute]
        public List<Player> Players { get; set; }

        public GameState() {}

        public GameState(IMap map, List<Player> players)
        {
            Map = map;
            Players = players;
        }
    }
}
