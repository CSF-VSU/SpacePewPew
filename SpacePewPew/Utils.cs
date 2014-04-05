using System.Drawing;

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
        GameMenu,
        Pause,
        Options
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

    public static class Additional
    {
        public static PointF NewPoint(PointF pos)  //конвертирование координат
        {
            return new PointF(pos.X / Consts.OGL_WIDTH * Consts.RIGHT, pos.Y / Consts.OGL_HEIGHT * Consts.MAP_HEIGHT);
        }
    }
}
