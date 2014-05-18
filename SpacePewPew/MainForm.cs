using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.GameLogic;
using SpacePewPew.UI;
using Tao.OpenGl;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        public Game SpacePew;
        public static Drawer OglDrawer;
        public LayoutManager LayoutManager;
        private PointF _mousePoint;

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
            var point = Additional.NewPoint(e.X, e.Y);
            switch (e.Button)
            {
                case MouseButtons.Left:
                   
                    if (!(LayoutManager.OnClick(point)))
                        if (LayoutManager.ScreenType == ScreenType.Game)
                            SpacePew.MouseClick(OglDrawer.ScreenToCell(point));
                    break;
                case MouseButtons.Right:
                    if (LayoutManager.ScreenType == ScreenType.Game)
                    {
                        if (SpacePew.CanBuildHere(OglDrawer.ScreenToCell(point)))
                        {
                            (LayoutManager.Components["Shop Menu"] as UI.ListView).Visible = true;
                            (LayoutManager.Components["Buy Ship"] as GameButton).Enabled = false;
                            SpacePew.IsShowingModal = true;
                            SpacePew.BuildingCoordinate = OglDrawer.ScreenToCell(point);
                        }
                    }
                    break;
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            if (LayoutManager.GetManager().ScreenType == ScreenType.Game)
            {
                OglDrawer.ScreenToCell(Additional.NewPoint(_mousePoint));
                SpacePew.Tick();
            }

            OglDrawer.Draw(LayoutManager, SpacePew.GetGameField());
            OGL.Invalidate();
        }

        private void OGL_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePoint = new PointF(e.X, e.Y);

            /*var tmp = Additional.NewPoint(new PointF(e.X, e.Y));
            var tmp1 = OglDrawer.ScreenToCell(tmp);
            Text = String.Format("Mouse {0:00.###};  {1:00.###}    Cell: {2};{3}", tmp.X, tmp.Y, tmp1.X, tmp1.Y);*/
        }
    }
}
