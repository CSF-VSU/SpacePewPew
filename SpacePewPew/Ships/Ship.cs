namespace SpacePewPew.Ships
{
    public abstract class Ship
    {
        #region Declarations
        public int MaxHealth { get; set; }
        public int Health { get; set; }

        public int Exp { get; set; }
        public int NextLvlExp { get; set; }

        public int Speed { get; set; }
        public TurnState HasTurn { get; set; }
        #endregion

        #region Actions
        public abstract void Move();
        public abstract void Shoot();
        #endregion
    }
}
