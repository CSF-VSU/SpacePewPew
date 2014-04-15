namespace SpacePewPew.GameObjects.Ships
{
    public abstract class Ship
    {
        #region Declarations
        public int id { get; set; }

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

 //       public int Orientation { get; set; } // 0 - 30deg, 1 - 90 deg .. 5 - 330deg
        #endregion
    }
}
