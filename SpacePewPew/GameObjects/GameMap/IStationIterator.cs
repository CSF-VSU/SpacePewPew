using SpacePewPew.GameObjects.MapObjects;

namespace SpacePewPew.GameObjects.GameMap
{
    public interface IStationIterator
    {
        Station First();
        Station Next();
        bool IsDone { get; }
        Station CurrentStation { get; }
    }
}
