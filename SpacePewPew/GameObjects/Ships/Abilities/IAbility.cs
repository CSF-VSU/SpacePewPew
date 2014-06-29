using System.Drawing;
using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.GameObjects.Ships.Abilities
{
    public interface IAbility
    {
        void Perform(IMapAbilityView map, Point coords);
    }
}
