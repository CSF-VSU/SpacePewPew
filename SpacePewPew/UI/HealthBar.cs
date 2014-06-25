using System.Drawing;


namespace SpacePewPew.UI
{
    public class HealthBar : UiElement
    {
        public PointF Position { get; set; }
        public override void Draw()
        {
            throw new System.NotImplementedException();
        }

        public override bool Click(PointF pos)
        {
            throw new System.NotImplementedException();
        }

        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        

        public HealthBar(int maxHealth, PointF pos)
        {
            CurrentHealth = MaxHealth = maxHealth;
            Position = pos;
        }
    }
}