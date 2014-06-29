using System;
using SpacePewPew.DataTypes;

namespace SpacePewPew.GameObjects.Ships
{
    [Serializable]
    public class Ship : ICloneable
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
        public int RemainedSpeed { get; set; }
        public TurnState TurnState { get; set; }

        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Volleys { get; set; }


        #endregion

        /*public Ship(int id)
        {
            Id = id;
        }*/

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
