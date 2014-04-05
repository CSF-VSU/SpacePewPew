namespace SpacePewPew.GameObjects.MapObjects
{
    class Station : IObstacle
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
