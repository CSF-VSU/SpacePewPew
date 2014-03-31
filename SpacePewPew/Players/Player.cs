using SpacePewPew.Players.Strategies;

namespace SpacePewPew.Players
{
    public class Player
    {
        public PlayerColor PlayerColor { get; set; }
        public int Money { get; set; }

        public int TurnTime { get; set; }
        public int MaxTurnTime { get; set; }

        public int UnitCount { get; set; }
        public int StationCount { get; set; }

        public bool IsHuman { get; set; }

        public Strategy Strategy;

        public Player(PlayerColor color)
        {
            PlayerColor = color;
            Money = 0;
            TurnTime = 0;
            MaxTurnTime = 210;
            UnitCount = 0;
            StationCount = 0;
            IsHuman = true;
        }

    }
}
