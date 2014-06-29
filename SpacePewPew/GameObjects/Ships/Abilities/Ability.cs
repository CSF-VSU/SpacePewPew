using System.Drawing;
using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.GameObjects.Ships.Abilities
{
    interface IAbility
    {
        void Perform(IMapView map, Point coords);
    }
}
