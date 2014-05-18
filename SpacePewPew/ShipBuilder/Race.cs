using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.ShipBuilder
{
    public class Race
    {
        private readonly RaceName _raceName ;
        public string Name { get; set; }
        public string Description { get; set; }
        
        public Race(RaceName raceName)
        {
            _raceName = raceName;
            switch (raceName)
            {
                case RaceName.Human :
                    Name = "Human";
                    Description = "Typical humanssss";
                    break;
                case RaceName.Swarm:
                    Name = "Swarm";
                    Description = "Another race";
                    break;
                case RaceName.Dentelian:
                    Name = "Dentelian";
                    Description = "And another one";
                    break;
                case RaceName.Kronolian:
                    Name = "Kronolian";
                    Description = "How ambitious are we! 4 races!";
                    break;
            }
        }


        public Ship BuildShip(int index)
        {
            return ShipCreator.GetCreator().BuildShip(new ShipNum {Index = index, Race = _raceName});
        }
    }
}
