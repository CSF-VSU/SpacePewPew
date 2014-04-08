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
        public int Tick;

        public MainForm()
        { 
            InitializeComponent();
            OGL.InitializeContexts();
            Tick = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SpacePew = Game.Instance();
            OglDrawer = Drawer.Instance();
            LayoutManager = new LayoutManager(SpacePew.GameScreen);
            OglDrawer.Initialize();
            timer1.Enabled = true;
        }


        private void OGL_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    SpacePew.MouseClick(new Point(e.X, e.Y));
                    LayoutManager.OnClick(Additional.NewPoint(new PointF(e.X, e.Y)));
                    Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Tick++;
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            OglDrawer.LightenedCell = SpacePew._map.LightenedCell;
            OglDrawer.Draw(SpacePew.GameState, LayoutManager);
            OGL.Invalidate();
        }

        private void OGL_MouseMove(object sender, MouseEventArgs e)
        {
            var tmp = Additional.NewPoint(new PointF(e.X, e.Y));
            var tmp1 = SpacePew._map.ConvertPoint(tmp);
            Text = String.Format("Mouse {0:00.###};  {1:00.###}    Cell: {2};{3}", tmp.X, tmp.Y, tmp1.X, tmp1.Y);
        }
    }
}
