using System.Drawing;

namespace SpacePewPew
{
    public struct PlayerInfo
    {
        public PlayerColor Color { get; set; }
        public int Ships { get; set; }
        public int Stations { get; set; }
        public int Money { get; set; }
    }
   

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
        Pause,
        Options,
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
        Rotating,
        Moving
    }


    public static class Additional
    {
        public static PointF NewPoint(PointF pos)  //конвертирование координат
        {
            return new PointF(pos.X / Consts.OGL_WIDTH * Consts.SCREEN_WIDTH, pos.Y / Consts.OGL_HEIGHT * Consts.SCREEN_HEIGHT);
        }
    }
}
