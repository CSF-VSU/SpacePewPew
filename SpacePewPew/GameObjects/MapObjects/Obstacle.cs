using System;

namespace SpacePewPew.GameObjects.MapObjects
{
    [Serializable]
    public abstract class IObject
    {
        public bool     IsPassable      { get; set; }
        public bool     IsDestructable  { get; set; }
    }
}
