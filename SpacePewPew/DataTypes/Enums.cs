namespace SpacePewPew
{
    public enum RaceName
    {
        Human,
        Dentelian,
        Swarm,
        Kronolian
    }

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
        Game,
        Editor
    }

    public enum TurnState
    {
        Ready,
        InAction,
        Finished
    }

    public enum PlayerColor
    {
        None,
        Red,
        Blue,
        Green,
        Orange
    }

    public enum ActionState
    {
        None,
        Attack,
        Rotating,
        Moving
    }
}
