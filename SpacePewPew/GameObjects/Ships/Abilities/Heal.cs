using System.Drawing;
using System.Linq;
using SpacePewPew.DataTypes;
using SpacePewPew.GameLogic;
using SpacePewPew.GameObjects.GameMap;
using SpacePewPew.GameObjects.Ships.Abilities.AbilityContainer;

namespace SpacePewPew.GameObjects.Ships.Abilities
{
    public class Heal : IAbility
    {
        public AbilityResult Perform(IMapAbilityView map, Point coords)
        {
            var ships = map.GetShipsAround(coords);
            var myColor = Game.Instance().CurrentPlayer.Color;

            var result = new AbilityResult{Name = AbilityName.Heal, Invoker = coords};
            
            foreach (var ship in ships.Where(ship => ship.Color == myColor && ship.Health < ship.MaxHealth))
            {
                result.Area.Add(map.GetShipPosition(ship));
                ship.Health += Consts.HEAL_POWER;
            }
            
            return result;
        }
    }
}
