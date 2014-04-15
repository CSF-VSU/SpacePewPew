using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.Players.Strategies
{
    class HumanStrategy : Strategy
    {
        public override Decision MakeDecision(Map map)
        {
            return new Decision {DecisionType = DecisionType.Halt};
        }
    }
}
