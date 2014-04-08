using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.GameObjects.GameMap
{
    public interface IShipIterator
    {
        Ship First();
        Ship Next();
        bool IsDone { get; }
        Ship CurrentShip { get; }
    }
}
