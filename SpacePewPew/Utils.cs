using System.Drawing;

namespace SpacePewPew
{
    public delegate void ButtonDrawDelegate(PointF position);

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
        public static PointF newPoint(PointF pos)  //конвертирование координат
        {
            return new PointF((float)pos.X / Consts.OGL_WIDTH * Consts.RIGHT, (float)pos.Y / Consts.OGL_HEIGHT * 100);
        }
    }
}
