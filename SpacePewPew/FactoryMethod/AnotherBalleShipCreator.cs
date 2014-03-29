using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.FactoryMethod
{
    class AnotherBalleShipCreator : Creator
    {
        public override Ship FactoryMethod()
        {
            return new AnotherBattleShip();
        }
    }
}
