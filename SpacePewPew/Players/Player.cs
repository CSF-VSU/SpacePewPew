using SpacePewPew.Players.Strategies;

﻿namespace SpacePewPew.Players
{
    public class Player
    {
        public PlayerColor Color { get; set; }
        public int Money { get; set; }

        public int TimeLeft { get; set; }
        public int MaxTurnTime { get; set; }

        public int UnitCount { get; set; }
        public int StationCount { get; set; }

        public bool IsHuman { get; set; }

        public Strategy Strategy;

        public Player(PlayerColor color, bool isHuman)
        {
            Color = color;
            Money = 0;
            TimeLeft = 210      * 25;
            MaxTurnTime = 210   * 25;
            UnitCount = 0;
            StationCount = 0;
            IsHuman = isHuman;
        }

    }
}
