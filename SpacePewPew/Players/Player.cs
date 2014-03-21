namespace SpacePewPew.Players
{
    abstract public class Player
    {
        public PlayerColor PlayerColor { get; set; }
        public int Money { get; set; }

        public int TurnTime { get; set; }
        public int MaxTurnTime { get; set; }

        public int UnitCount { get; set; }
        public int StationCount { get; set; }

        public abstract void MakeTurn(ref bool keyboardEnabled);
    }
}
