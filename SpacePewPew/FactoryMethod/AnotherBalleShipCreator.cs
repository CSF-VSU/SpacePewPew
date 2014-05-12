using System;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.FactoryMethod
{
    [Serializable]
    class AnotherBalleShipCreator : Creator
    {
        public override Ship FactoryMethod()
        {
            return new AnotherBattleShip(GetNextId());
        }
    }
}
