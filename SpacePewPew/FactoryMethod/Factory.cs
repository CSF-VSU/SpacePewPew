/*ing System.Collections.Generic;

namespace SpacePewPew.FactoryMethod
{
    public class Factory
    {
        public struct FactoryItem
        {
            public Creator Creator { get; set; }
            public int Cost { get; set; }
            public string Name { get; set; }
        }

        public Dictionary<Race, FactoryItem[]> Builder { get; set; }

        public Factory()
        {
            Builder = new Dictionary<Race, FactoryItem[]> {{Race.Race1, new FactoryItem[2]}};
            Builder[Race][0].Creator = new BattleShipCreator();
            Builder[Race.Race1][0].Cost = 25;
            Builder[Race.Race1][0].Name = "BattleShip";

            Builder[Race.Race1][0].Creator = new AnotherBalleShipCreator();
            Builder[Race.Race1][0].Cost = 30;
            Builder[Race.Race1][0].Name = "AnotherBattleShip";
        }
    }
}
*/