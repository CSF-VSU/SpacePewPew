using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    public interface IMapAbilityView
    {
        Cell[,] MapCells { get; set; }

        bool IsBuildingArea(Point buildingCoordinate);
        
        IEnumerable<Ship> GetShipsAround(Point p);
        bool HasEnemyIn(IEnumerable<Ship> ships);
        Ship GetShipFromPoint(Point p);
        Point GetShipPosition(Ship s);
    }
}
