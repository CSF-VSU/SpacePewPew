using System;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        internal Game SpacePew;

        public MainForm()
        {
            InitializeComponent();
            OGL.InitializeContexts();
            SpacePew = Game.Instance();
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
    }
}
