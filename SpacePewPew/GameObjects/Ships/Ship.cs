using System;
using System.Collections.Generic;
using SpacePewPew.DataTypes;
using SpacePewPew.GameObjects.Ships.ActiveEffects;

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

        private int _health;
        public int Health 
        {
            get { return _health; }
            set
            {
                if (value > MaxHealth) _health = MaxHealth;
                else if (value < 0) _health = 0;
                else _health = value;
            } 
        }

        public int Exp { get; set; }
        public int NextLvlExp { get; set; }

        public int Speed { get; set; }
        public int RemainedSpeed { get; set; }
        public TurnState TurnState { get; set; }

        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Volleys { get; set; }

        public List<AbilityName> Abilities { get; set; }
        public List<ActiveEffect> Status { get; set; }  
        #endregion

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
