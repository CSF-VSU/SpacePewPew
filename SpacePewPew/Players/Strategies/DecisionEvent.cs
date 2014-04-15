using System;

namespace SpacePewPew.Players.Strategies
{
    public class DecisionArgs : EventArgs
    {
        public Decision Decision { get; set; }
    }

    public delegate void DecisionHandler(object sender, DecisionArgs e);
}
