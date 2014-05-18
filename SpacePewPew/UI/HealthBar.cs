using System.Drawing;


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
