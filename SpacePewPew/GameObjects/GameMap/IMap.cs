using System.Drawing;

namespace SpacePewPew.GameObjects.GameMap
{
    public interface IMap
    {
        PointF LightenedCell { get; set; }
        Cell[,] MapCells { get; set; }
    }
}
