using System;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public class Planet : IObject
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
