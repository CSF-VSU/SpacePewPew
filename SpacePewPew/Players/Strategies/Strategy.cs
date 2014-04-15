using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.Players.Strategies
{
    public abstract class Strategy
    {
        public abstract Decision MakeDecision(Map map);
    }
}