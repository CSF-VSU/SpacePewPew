using System.Drawing;

namespace SpacePewPew
{
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
        Attack,
        Rotating,
        Moving
    }


    public static class Additional
    {
        public static PointF NewPoint(PointF pos)  //конвертирование координат
        {
            return new PointF(pos.X / Consts.OGL_WIDTH * Consts.SCREEN_WIDTH, pos.Y / Consts.OGL_HEIGHT * Consts.SCREEN_HEIGHT);
        }

        public static string ConvertTime(int tick) // преобразует время в тиках в секунды
        {
            var sec = tick/25;
            var min = sec/60;
            sec = sec%60;
            return sec < 10 ?  min + ":0" + sec : min + ":" + sec;
        }
    }
}
