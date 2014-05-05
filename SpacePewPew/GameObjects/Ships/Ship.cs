using System;

namespace SpacePewPew.GameObjects.Ships
{
    [Serializable]
    public abstract class Ship
    {
        #region Declarations
        public int Id { get; set; }

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

        public int DamagePerTime { get; set; }
        public int NumberOfAttacks { get; set; }

        #endregion

        public Ship(int id)
        {
            Id = id;
        }
    }
}
