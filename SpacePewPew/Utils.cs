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


    /*float relX = p.X / (float)(1.5 * Consts.CELL_SIDE); // 1.
        int mapX = (int)relX; // 1
        float relY;
        if (mapX % 2 != 0)
            relY = p.Y / (float)(Math.Sqrt(3) * Consts.CELL_SIDE);
        else
            relY = (p.Y - 0.5f * Consts.CELL_SIDE) / (float)(Math.Sqrt(3) * Consts.CELL_SIDE);

        bool hasConflictBorder = relX < 0.5 * Consts.CELL_SIDE;
        int mapY = (int)relY + 1;
        bool hasVertShift = mapX / 2 != 0;

        if (!hasConflictBorder)
        {
            if (!hasVertShift)
                mapY = (int)(p.Y / (float)(Math.Sqrt(3) * Consts.CELL_SIDE));
            else
                mapY = (int)((p.Y + Math.Sqrt(3) / 2 * Consts.CELL_SIDE) / (Consts.CELL_SIDE * Math.Sqrt(3)));
        }
        else
        {
            float d = (float)Math.Sqrt(3) / 2 * Consts.CELL_SIDE - relY;

            if (!hasVertShift)
            {
                float tga = Math.Abs(d) / relX;
                if (tga > Math.Sqrt(3))
                    mapX--;
                if (tga > Math.Sqrt(3) && d > 0)
                    mapY--;                    
            }
            else
            {
                float tga = Math.Abs(d) / ((float)0.5 * Consts.CELL_SIDE - relX);
                if (tga < Math.Sqrt(3))
                    mapX --;
                if (tga < Math.Sqrt(3) && d < 0)
                    mapY--;
            }
        }

        return new Point(mapX, mapY);*/
    // PointF p = new PointF(12, 9);m

}
