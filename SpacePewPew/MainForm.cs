using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            OGL.InitializeContexts();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // инициализация Glut 
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            // очитка окна 
            Gl.glClearColor(255, 255, 255, 1);

            // установка порта вывода в соотвествии с размерами элемента OGL 
            Gl.glViewport(0, 0, OGL.Width, OGL.Height);

            // настройка проекции 
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            // теперь необходимо корректно настроить 2D ортогональную проекцию 
            // в зависимости от того, какая сторона больше 
            // мы немного варьируем то, как будет сконфигурированный настройки проекции
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
