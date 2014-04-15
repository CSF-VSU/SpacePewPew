using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.Players.Strategies
{
    class AIStrategy : Strategy
    {
        public override Decision MakeDecision(Map map)
        {
            return new Decision {DecisionType = DecisionType.Move, ShipIndex = 0};
        }
    }
}
