using System;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public class Dock : IObject
    {
        public Dock()
        {
            IsDestructable = false;
            IsPassable = true;
        }
    }
}
