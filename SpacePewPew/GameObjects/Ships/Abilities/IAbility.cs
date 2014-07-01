using System.Drawing;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.GameObjects.Ships.Abilities.AbilityContainer;

namespace SpacePewPew.GameObjects.Ships.Abilities
{
    public interface IAbility
    {
        AbilityResult Perform(IMapAbilityView map, Point coords);
    }
}
