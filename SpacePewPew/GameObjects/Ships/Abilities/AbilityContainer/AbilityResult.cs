using System.Collections.Generic;
using System.Drawing;
using SpacePewPew.DataTypes;

namespace SpacePewPew.GameObjects.Ships.Abilities.AbilityContainer
{
    public class AbilityResult
    {
        public AbilityName Name { get; set; }
        public List<Point> Area { get; set; }
        public Point Invoker { get; set; }

        public AbilityResult()
        {
            Area = new List<Point>();
        }
    }
}
