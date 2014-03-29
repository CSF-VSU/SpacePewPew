using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.UI;
using Tao.FreeGlut;
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

            SpacePew = Game.Instance();
            OglDrawer = Drawer.Instance();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Gl.glClearColor(255, 255, 255, 1);

            Gl.glViewport(0, 0, OGL.Width, OGL.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            if (OGL.Width <= (float)OGL.Height)
            {
                Glu.gluOrtho2D(0.0, 30.0 * OGL.Height / OGL.Width, 0.0, 30.0);
            }
            else
            {
                var right = 30 * (float)OGL.Width / OGL.Height;
                //             left,right,bottom,top;
                Glu.gluOrtho2D(0.0, right, 0.0, 30.0);
            }

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            //чекнуть, не попал ли в лэйаут-элемент, ЭЛС

            SpacePew.MouseClick(new Point(e.X, e.Y));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            OglDrawer.Draw(SpacePew.GameState);
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            //дергать не каждый раз
        }

        // log 4 net


    }
}
