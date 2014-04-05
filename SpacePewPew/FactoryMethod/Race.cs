namespace SpacePewPew.FactoryMethod
{
    public class Race
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Creator[] Builder { get; set; }

        public Race(RaceName raceName)
        {
            Builder = new Creator[6];

            switch (raceName)
            {
                case RaceName.Human :
                    Name = "Human";
                    Description = "Typical humanssss";

                    Builder[0] = new BattleShipCreator();
                    Builder[1] = new AnotherBalleShipCreator();
                    break;
                case RaceName.Swarm:
                    Name = "Swarm";
                    Description = "Breed is immortal";
                    break;
                case RaceName.Dentelian:
                    Name = "Dentelian";
                    Description = "Will crush you.. slowly";
                    break;
                case RaceName.Kronolian:
                    Name = "Kronolian";
                    Description = "In mind we trust";
                    break;
            }
        }

    }
}
