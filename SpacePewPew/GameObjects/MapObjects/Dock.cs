namespace SpacePewPew.GameObjects.MapObjects
{
    public class Dock : IObstacle
    {
        public Dock()
        {
            IsDestructable = false;
            IsPassable = true;
        }
    }
}
