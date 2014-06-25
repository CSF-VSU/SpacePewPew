using System.Drawing;

namespace SpacePewPew.UI
{
    public class HealthBar : UiElement //TODO: по-хорошему, HealthBar должен наследоваться от Sprite'а какого-нибудь, а не от UiElement'а
    {
        public override void Draw()
        {
            //
        }

        public override bool Click(PointF pos)
        {
            return false;//throw new System.NotImplementedException();
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