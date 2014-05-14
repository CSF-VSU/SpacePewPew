namespace SpacePewPew.Prototype
{
    public class ShipInfo
    {
        /*
        public int Cost { get; set; }

        public int Exp { get; set; }
        public int NextLvlExp { get; set; }

        public TurnState HasTurn { get; set; }
        */

        public RaceName Race { get; set; }
        public int Index { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }

        public int Cost { get; set; }

        public int HP { get; set; }
        public int Speed { get; set; }
        
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int Volleys { get; set; }
    }
}
