namespace SpacePewPew.GameObjects.MapObjects
{
    class Station : Obstacle
    {
        public PlayerColor OwnerColor { get; set; }
        public int Income { get; set; }

        public Station()
        {
            IsPassable = true;
            IsDestructable = false;
            OwnerColor = PlayerColor.None;
            Income = 5;
        }

        public void Capture(PlayerColor color)
        {
            OwnerColor = color;
        }
    }
}
