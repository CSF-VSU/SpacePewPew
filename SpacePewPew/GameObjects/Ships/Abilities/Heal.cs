using System.Drawing;
using System.Linq;
using SpacePewPew.GameLogic;
using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.GameObjects.Ships.Abilities
{
    public class Heal : IAbility
    {
        public void Perform(IMapAbilityView map, Point coords)
        {
            var ships = map.GetShipsAround(coords);
            var myColor = Game.Instance().CurrentPlayer.Color;

            foreach (var ship in ships.Where(ship => ship.Color == myColor && ship.Health < ship.MaxHealth))
            {
                ship.Health += Consts.HEAL_POWER;
            }
        }
    }
}
