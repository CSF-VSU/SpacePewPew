using System;
using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.Players.Strategies
{
    [Serializable]
    class AIStrategy : Strategy
    {
        public override Decision MakeDecision(Map map)
        {
            return new Decision {DecisionType = DecisionType.Move, ShipIndex = 0};
        }
    }
}
