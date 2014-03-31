using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.UI;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SpacePewPew
{
    //public delegate void ButtonDrawDelegate(PointF position);
    

    public partial class MainForm : Form
    {
        internal Game SpacePew;
        internal Drawer OglDrawer;
        internal LayoutManager LayoutManager;


        public ButtonDrawDelegate DrawButton;

        public MainForm()
        { 
            InitializeComponent();
            OGL.InitializeContexts();

            
        }


        /* Для перевода в свою систему координат 
         * e1.X = (float)e1.X/OGL.Width*right; right = 100 * (Heigth/Width) || (Width/heigth) (что больше)
         * e1.Y = (float)e1.Y/OGL.Heigth*100
        */
        private void MainForm_Load(object sender, EventArgs e)
        { 
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Gl.glClearColor(0.8f, 0.8f, 0.8f, 1);

            Gl.glViewport(0, 0, OGL.Width, OGL.Height);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();
       /*     if (OGL.Width <= OGL.Height) 
                right = 100 * (float)OGL.Height / OGL.Width; 
            else 
                right = 100 * (float)OGL.Width / OGL.Height;  */
            Glu.gluOrtho2D(0.0, Consts.RIGHT, 100.0, 0.0);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            SpacePew = Game.Instance();
            OglDrawer = Drawer.Instance(DB);
            LayoutManager = new LayoutManager(SpacePew.GameScreen);
            
        //    OglDrawer.draw = DB;
            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            OglDrawer.Draw(SpacePew.GameState);
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
                LayoutManager.OnClick(Additional.newPoint(new PointF(e.X, e.Y)));
                Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            }   
        }


        #region DrawingStuff
        private void DB(PointF pos) //рисовка кнопки
        {
            Gl.glColor3f(0, 0, 0);

            Gl.glBegin(Gl.GL_POLYGON);
           //  var tmp = new Additional
            Gl.glVertex2d(pos.X, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glVertex2d(pos.X, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glEnd();

            Gl.glFlush();
            OGL.Invalidate();
        }


       // public NoParamsDelegate = Close 
        #endregion



    }
}
