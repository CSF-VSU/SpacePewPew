﻿using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.GameLogic;
using Tao.OpenGl;

namespace SpacePewPew.UI.Proxy
{
    public class Proxy
    {
        public LayoutManager Manager { get; set; }
        private readonly Game _game;
        private readonly Drawer _drawer;
        private PointF _mousePoint;

        #region Singleton

        private Proxy()
        {
            Manager = LayoutManager.GetManager();
            _game = Game.Instance();
            _drawer = Drawer.Instance();
        }

        public static Proxy GetInstance()
        {
            return _ins ?? (_ins = new Proxy());
        }

        private static Proxy _ins;

        #endregion

        public PointF NewPoint(PointF pos)  //конвертирование координат
        {
            return new PointF(pos.X / Consts.OGL_WIDTH * Consts.SCREEN_WIDTH, pos.Y / Consts.OGL_HEIGHT * Consts.SCREEN_HEIGHT);
        }

        public PointF NewPoint(float x, float y)
        {
            return new PointF(x / Consts.OGL_WIDTH * Consts.SCREEN_WIDTH, y / Consts.OGL_HEIGHT * Consts.SCREEN_HEIGHT);
        }

        public string ConvertTime(int tick) // преобразует время в тиках в секунды
        {
            var sec = tick / 25;
            var min = sec / 60;
            sec = sec % 60;
            return sec < 10 ? min + ":0" + sec : min + ":" + sec;
        }


        public void OnClick(Point location, MouseButtons mouseButton)
        {
            var newPoint = NewPoint(location);

            switch (mouseButton)
            {
                case MouseButtons.Left:
                    if (!(Manager.OnClick(newPoint)))
                        if (Manager.ScreenType == ScreenType.Game)
                            _game.MouseClick(_drawer.ScreenToCell(newPoint));
                    break;

                case MouseButtons.Right:
                    if (Manager.ScreenType == ScreenType.Game)
                    {
                        if (!_game.CanBuildHere(_drawer.ScreenToCell(newPoint))) return;
                        
                        Manager.IsShowingModal = true;
                        _game.BuildingCoordinate = _drawer.ScreenToCell(newPoint);
                    }
                    break;
            }
        }

        public void Tick()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            if (Manager.ScreenType == ScreenType.Game)
            {
                Drawer.Instance().ScreenToCell(NewPoint(_mousePoint));
                Game.Instance().Tick();
            }

            Drawer.Instance().Draw(Manager, Game.Instance().GetGameField());
        }

        public void GetMousePos(int x, int y)
        {
            _mousePoint = new PointF(x, y);
        }
    }
}
