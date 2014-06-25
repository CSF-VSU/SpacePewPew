namespace SpacePewPew.DataTypes
{
    public struct ListViewItemData
    {
        public uint GlyphNum { get; set; }
        public string Text { get; set; }
        public string Appendix { get; set; }
    }

    public struct AttackInfo
    {
        public int Damage { get; set; }
        public bool IsDestroyed { get; set; }
        //public string Debuff { get; set; }
        public bool IsMineAttack { get; set; }
    }

    public struct ShipNum
    {
        public RaceName Race { get; set; }
        public int Index { get; set; }
    }
}
