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

    }
}
