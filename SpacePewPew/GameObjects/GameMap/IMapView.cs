using System.Drawing;
using System.Collections.Generic;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    public interface IMapView
    {
        Point ChosenShip { get; set; }
        Cell[,] MapCells { get; set; }
        List<Point> Lightened { get; set; }

        IEnumerable<Ship> GetShipIterator(PlayerColor color);
        IEnumerable<Ship> GetShipIterator();
    }
}
