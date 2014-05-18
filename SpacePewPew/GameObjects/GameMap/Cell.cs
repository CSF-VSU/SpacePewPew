using System;
using System.Drawing;
using SpacePewPew.GameObjects.MapObjects;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    [Serializable]
    public struct Cell
    {
        public Ship Ship { get; set; }
        public IObject Object { get; set; }
        public bool IsLightened { get; set; }
        public Point Previous { get; set; }
        public bool Visited { get; set; }
    }
}
