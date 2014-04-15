using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.UI;
using Tao.OpenGl;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        public Game SpacePew;
        public Drawer OglDrawer;
        public LayoutManager LayoutManager;
        //public int Tick;

        public MainForm()
        { 
            InitializeComponent();
            OGL.InitializeContexts();
            //Tick = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SpacePew = Game.Instance();
            OglDrawer = Drawer.Instance();
            SpacePew.Init(OglDrawer);
            
            LayoutManager = new LayoutManager(SpacePew.GameScreen);
            OglDrawer.Initialize();
            timer1.Enabled = true;
        }

        private void OGL_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!(LayoutManager.OnClick(Additional.NewPoint(new PointF(e.X, e.Y)))))
                    SpacePew.MouseClick(OglDrawer.ScreenToCell(Additional.NewPoint((new PointF(e.X, e.Y)))));

                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            }

            /*switch (LayoutManager.ScreenType)
            {
                case ScreenType.MainMenu:
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (!(LayoutManager.OnClick(Additional.NewPoint(new PointF(e.X, e.Y)))))
                            SpacePew.MouseClick(OglDrawer.ScreenToCell(Additional.NewPoint((new PointF(e.X, e.Y)))));

                        Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                    }
                    break;
                }
                case ScreenType.Game:
                {*/
                   /* var tmp = OglDrawer.ScreenToCell(Additional.NewPoint(new PointF(e.X, e.Y)));
                    if (_action.StartPos.X == -1)
                    {
                        if (SpacePew._map.MapCells[tmp.X, tmp.Y].Ship != null) //первый клик
                        {
                            _action.StartPos = new Point(tmp.X, tmp.Y);
                            _action.ShipId = SpacePew._map.MapCells[tmp.X, tmp.Y].Ship.id;
                            Last = tmp;
                        }
                    }
                    else //второй клик, проверить есть ли объект
                    {
                        _action.Destination = new Point(tmp.X, tmp.Y);  //неявный флаг запуска анимации
                        _action.IsReady = true;
                        _action.Path = SpacePew._map.FindWay(_action.StartPos, _action.Destination);   //тут же происходит корректировка пути с условием, что есть корабль
                        //if !(клетка занята другим кораблем, то)
                        SpacePew._map.MapCells[tmp.X, tmp.Y].Ship = SpacePew._map.MapCells[Last.X, Last.Y].Ship;
                        SpacePew._map.MapCells[Last.X, Last.Y].Ship = null;
                    }*///AND WILL NEVER BE USED MADAFUCKER
                  /*  break;
                }
            }*/
        }


        Action _action = new Action();  
        private void timer1_Tick(object sender, EventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            
            OglDrawer.Draw(SpacePew.GameState, LayoutManager, SpacePew.GetGameField(), ref _action);
            OGL.Invalidate();
        }

        private void OGL_MouseMove(object sender, MouseEventArgs e)
        {
            var tmp = Additional.NewPoint(new PointF(e.X, e.Y));
            var tmp1 = OglDrawer.ScreenToCell(tmp);
            Text = String.Format("Mouse {0:00.###};  {1:00.###}    Cell: {2};{3}", tmp.X, tmp.Y, tmp1.X, tmp1.Y);
        }
    }
}
