using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.UI;
using Tao.OpenGl;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        internal Game SpacePew;
        internal Drawer OglDrawer;
        internal LayoutManager LayoutManager;

        public MainForm()
        { 
            InitializeComponent();
            OGL.InitializeContexts();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {           
            SpacePew = Game.Instance();
            OglDrawer = Drawer.Instance();
            LayoutManager = new LayoutManager(SpacePew.GameScreen);
            OglDrawer.Initialize();
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            //дергать не каждый раз
        }

        private void OGL_MouseClick(object sender, MouseEventArgs e)
        { 
            if (e.Button == MouseButtons.Left)
            {
                SpacePew.MouseClick(new Point(e.X, e.Y));
                LayoutManager.OnClick(Additional.NewPoint(new PointF(e.X, e.Y)));
            }   
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            OglDrawer.Draw(SpacePew, LayoutManager);
            OGL.Invalidate();
        }
    }
}
