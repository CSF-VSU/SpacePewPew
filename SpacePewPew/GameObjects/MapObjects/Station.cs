using System;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public class Station : IObject
    {
        public PlayerColor OwnerColor { get; set; }
        public int Income { get; set; }

        protected Station()
        {
            IsPassable = true;
            IsDestructable = false;
            Income = 5;
        }

        public Station(PlayerColor color) : this()
        {
            OwnerColor = color;
        }

        public void Capture(PlayerColor color)
        {
            OwnerColor = color;
        }
    }
}
