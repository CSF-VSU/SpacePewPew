namespace SpacePewPew.GameObjects.Ships
{
    public abstract class Ship
    {
        #region Declarations
        public PlayerColor Color { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Cost { get; set; }

        public int MaxHealth { get; set; }
        public int Health { get; set; }

        public int Exp { get; set; }
        public int NextLvlExp { get; set; }

        public int Speed { get; set; }
        public TurnState HasTurn { get; set; }
        #endregion
    }
}
