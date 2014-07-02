namespace SpacePewPew.DataTypes
{
    public enum RaceName
    {
        Human,
        Dentelian,
        Swarm,
        Kronolian
    }

    public enum AbilityName
    {
        Heal,
        Corrosion
    }

    public enum EffectName
    {
        Corrosion
    }

    public enum DecisionType
    {
        Move,
        Attack
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
