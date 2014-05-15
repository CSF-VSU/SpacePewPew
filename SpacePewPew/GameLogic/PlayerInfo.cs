using System.Linq;
using SpacePewPew.GameLogic;

namespace SpacePewPew
{
    public static class PlayerInfo
    {
        public static PlayerColor Color
        {
            get { return Game.Instance().CurrentPlayer.Color; }
        }

        public static int Ships
        {
            get { return Game.Instance().Map.GetShipIterator(Color).Count(); }
        }

        /*public static int Stations
        {
            get { return Game.Instance().Map.GetStationIterator(Color).Count(); }
        }*/

        public static int Money
        {
            get { return Game.Instance().CurrentPlayer.Money; }
        }

        public static int TimeLeft
        {
            get { return Game.Instance().CurrentPlayer.TimeLeft; }
        }
    }
}
