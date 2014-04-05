using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.UI;
using Tao.FreeGlut;
using Tao.OpenGl;
using Tao.DevIl;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        public Game SpacePew;
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
            PointF tmp = Additional.NewPoint(new PointF(e.X, e.Y));
            Point tmp1 = SpacePew._map.ConvertPoint(tmp);
            this.Text = "Mouse " + tmp.X + " " + tmp.Y + "  Cell " + tmp1.X + " " + tmp1.Y; 
        //    OglDrawer.drawString(new PointF(Consts.MAP_WIDTH - 10, Consts.MAP_HEIGHT + 10), "Mouse" + tmp.X + " " + tmp.Y);
        //    OglDrawer.drawString(new PointF(Consts.MAP_WIDTH - 10, Consts.MAP_HEIGHT + 20), "Cell" + tmp1.X + " " + tmp1.Y);
           // Gl.glFlush();
         //   OGL.Invalidate();
        }

        private void OGL_MouseClick(object sender, MouseEventArgs e)
        { 
            
            if (e.Button == MouseButtons.Left)
            {
                SpacePew.MouseClick(new Point(e.X, e.Y));
                LayoutManager.OnClick(Additional.NewPoint(new PointF(e.X, e.Y)));
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            }
        }




        private void timer1_Tick(object sender, EventArgs e)
        {


            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            OglDrawer.lightenedCell = SpacePew._map.lightenedCell;
            OglDrawer.Draw(SpacePew.GameState, LayoutManager);
            
            OGL.Invalidate();


        }

        private void OGL_MouseMove(object sender, MouseEventArgs e)
        {
            PointF tmp = Additional.NewPoint(new PointF(e.X, e.Y));
            Point tmp1 = SpacePew._map.ConvertPoint(tmp);
            this.Text = String.Format("Mouse {0:##.###};{1:##.###} Cell: {2};{3}", tmp.X, tmp.Y, tmp1.X, tmp1.Y);
    
        }
    }
}
