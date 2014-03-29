using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.FactoryMethod
{
    class BattleShipCreator : Creator
    {
        public override Ship FactoryMethod()
        {
            return new BattleShip();
        }
    }
}
