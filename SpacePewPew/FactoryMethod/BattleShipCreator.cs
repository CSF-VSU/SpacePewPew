using System;
using SpacePewPew.GameObjects.Ships;

namespace SpacePewPew.FactoryMethod
{
    [Serializable]
    public class BattleShipCreator : Creator
    {
        public int lastId { get; set; }
        public override Ship FactoryMethod()
        {
            return new BattleShip(GetNextId());
        }
    }
}
