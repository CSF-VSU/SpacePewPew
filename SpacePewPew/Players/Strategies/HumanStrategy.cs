using System;
using SpacePewPew.GameObjects.GameMap;

namespace SpacePewPew.Players.Strategies
{
    [Serializable]
    public class HumanStrategy : Strategy
    {
        public HumanStrategy()
        {
            ClickAppeared = false;
        }

        public override Decision MakeDecision(Map map)
        {
            if (!ClickAppeared)
            {
                return null;
            }
            ClickAppeared = false;
            return map.Click(MousePos);
        }
    }
}
