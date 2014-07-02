using System.Drawing;
using System.Collections.Generic;
using SpacePewPew.DataTypes;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    public interface IMapView
    {
        Point ChosenShip { get; set; }
        Cell[,] MapCells { get; set; }
        List<Point> Lightened { get; set; }

        IEnumerable<Ship> GetShips(PlayerColor color);
        IEnumerable<Ship> GetShips();
    }
}
