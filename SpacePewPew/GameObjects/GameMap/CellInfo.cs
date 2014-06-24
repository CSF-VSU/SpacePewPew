using System;
using SpacePewPew.GameObjects.MapObjects;

namespace SpacePewPew.GameObjects.GameMap
{
    [Serializable]
    public class CellInfo
    {
        public bool Enabled { get; set; }
        public IObject GameObject { get; set; }
        public int PlayerStart { get; set; }
    }
}
