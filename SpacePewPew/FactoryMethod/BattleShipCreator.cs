using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.FactoryMethod
{
    public class BattleShipCreator : Creator
    {
        public BattleShipCreator()
        {
            lastId = 0;
        }
        public int lastId { get; set; }
        public override Ship FactoryMethod()
        {
            lastId++;
            return new BattleShip();
            
        }
    }
}
