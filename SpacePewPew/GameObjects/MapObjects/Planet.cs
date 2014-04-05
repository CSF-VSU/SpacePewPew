namespace SpacePewPew.GameObjects.MapObjects
{
    public class Planet : IObstacle
    {
        public string Name { get; set; }

        public Planet(string name)
        {
            Name = name;
            IsDestructable = false;
            IsPassable = false;
        }
    }
}
