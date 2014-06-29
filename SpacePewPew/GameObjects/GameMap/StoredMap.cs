using System;

namespace SpacePewPew.GameObjects.GameMap
{
    [Serializable]
    public class StoredMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public CellInfo[,] MapData { get; set; }
        public int NumPlayers { get; set; }
    }
}
