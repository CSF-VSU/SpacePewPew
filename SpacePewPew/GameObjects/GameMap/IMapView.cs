using System.Drawing;
using System.Collections.Generic;

namespace SpacePewPew.GameObjects.GameMap
{
    public interface IMapView
    {
        Point ChosenShip { get; set; }
        Cell[,] MapCells { get; set; }
        List<Point> Lightened { get; set; }
    }
}
