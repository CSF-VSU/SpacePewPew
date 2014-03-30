namespace SpacePewPew
{
    public enum DecisionType
    {
        None,
        Halt,
        Move,
        Attack,
        Buy
    }

    public enum ScreenType
    {
        None,
        MainMenu,
        GameMenu,
        Pause,
        Options
    }

    /// <summary>
    /// Стадия хода юнита
    /// </summary>
    public enum TurnState
    {
        Ready,
        InAction,
        Finished
    }

    /// <summary>
    /// Цвет игрока
    /// </summary>
    public enum PlayerColor
    {
        None,
        Red,
        Blue,
        Green,
        Orange
    }
}
