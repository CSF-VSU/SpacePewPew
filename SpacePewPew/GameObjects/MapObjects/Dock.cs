using System;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public class Dock : IObstacle
    {
        public Dock()
        {
            IsDestructable = false;
            IsPassable = true;
        }
    }
}
