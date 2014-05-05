using System;
using SpacePewPew.GameObjects.GameMap;
using System.Drawing;

namespace SpacePewPew.Players.Strategies
{
    [Serializable]
    public abstract class Strategy
    {
        public bool ClickAppeared { get; set; }
        public Point MousePos { get; set; }

        public abstract Decision MakeDecision(Map map);
    }
}