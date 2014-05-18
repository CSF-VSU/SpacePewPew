using System;
using SpacePewPew.Players.Strategies;

﻿namespace SpacePewPew.Players
{
    [Serializable]
    public class Player
    {
        public PlayerColor Color { get; set; }
        public RaceName Race { get; set; }

        public int Money { get; set; }

        public int TimeLeft { get; set; }
        public int MaxTurnTime { get; set; } // TODO: вынести в константы

        public int UnitCount { get; set; }
        public int StationCount { get; set; }

        public bool IsHuman { get; set; }

        public Strategy Strategy;

        public Player(PlayerColor color, RaceName race, bool isHuman)
        {
            Color = color;
            Race = race;
            Money = 70;
            TimeLeft =      30 * 25;
            MaxTurnTime =   30 * 25;
            UnitCount = 0;
            StationCount = 1;
            IsHuman = isHuman;
            Strategy = new HumanStrategy();
        }

    }
}
