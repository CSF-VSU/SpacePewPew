using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.GameLogic;
using SpacePewPew.UI;
using Tao.OpenGl;
using ListView = SpacePewPew.UI.ListView;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        public Game SpacePew;
        public static Drawer OglDrawer;
        public LayoutManager LayoutManager;
        private Point mousePoint;

        public MainForm()
        { 
            InitializeComponent();
            OGL.InitializeContexts();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SpacePew = Game.Instance();
            OglDrawer = Drawer.Instance();
            SpacePew.Init(OglDrawer);
            
            LayoutManager = LayoutManager.GetManager();
            OglDrawer.Initialize();
            timer1.Enabled = true;
        }

        private void OGL_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!(LayoutManager.OnClick(Additional.NewPoint(new PointF(e.X, e.Y))))) //не попал в кнопку
                    if (LayoutManager.ScreenType == ScreenType.Game)
                        SpacePew.MouseClick(OglDrawer.ScreenToCell(Additional.NewPoint((new PointF(e.X, e.Y)))));

                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (LayoutManager.ScreenType == ScreenType.Game)
                {
                    (LayoutManager.Components["Shop Menu"] as ListView).Visible = !(LayoutManager.Components["Shop Menu"] as ListView).Visible;
                    SpacePew.IsShowingModal = true;
                    SpacePew.BuildingCoordinate = OglDrawer.ScreenToCell(Additional.NewPoint(new PointF(e.X, e.Y)));
                }
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            if (LayoutManager.GetManager().ScreenType == ScreenType.Game)
            {
                SpacePew.Tick();
                //TODO : подсветить клеточку под MouseCoord
            }

            OglDrawer.Draw(LayoutManager, SpacePew.GetGameField());

            OGL.Invalidate();
        }

        private void OGL_MouseMove(object sender, MouseEventArgs e)
        {
            //TODO : обновлять mouseCoord
            //mousePoint = new Point();
            
            var tmp = Additional.NewPoint(new PointF(e.X, e.Y));
            var tmp1 = OglDrawer.ScreenToCell(tmp);
            Text = String.Format("Mouse {0:00.###};  {1:00.###}    Cell: {2};{3}", tmp.X, tmp.Y, tmp1.X, tmp1.Y);
        }
    }
}
