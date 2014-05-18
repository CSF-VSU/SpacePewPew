using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace SpacePewPew.UI
{
    public class HealthBar : IUiElement
    {
        public PointF Position { get; set; }
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        

        public HealthBar(int maxHealth, PointF pos)
        {
            CurrentHealth = MaxHealth = maxHealth;
            Position = pos;
        }
    }
}
