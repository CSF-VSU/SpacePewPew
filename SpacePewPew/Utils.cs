using System.Drawing;

namespace SpacePewPew
{
    public delegate void DrawDelegate(PointF position);

    public enum Race
    {
        Race1,
        Race2
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

    public static class Additional
    {
        public static PointF NewPoint(PointF pos)  //конвертирование координат
        {
            return new PointF(pos.X / Consts.OGL_WIDTH * Consts.RIGHT, pos.Y / Consts.OGL_HEIGHT * 100);
        }
    }
}
