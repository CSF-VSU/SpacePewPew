using System.Drawing;
﻿using SpacePewPew.UI;
using Tao.DevIl;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SpacePewPew
{
    public class Drawer
    {
        #region Singleton pattern
        private static Drawer _instance;

        protected Drawer()
        {
        }

        public static Drawer Instance()
        {
            return _instance ?? (_instance = new Drawer());
        }
        #endregion

        public void Initialize()
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            Gl.glClearColor(1f, 1f, 1f, 1);

            Gl.glViewport(0, 0, Consts.OGL_WIDTH, Consts.OGL_HEIGHT);

            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            Glu.gluOrtho2D(0.0, Consts.RIGHT, 100.0, 0.0);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            Il.ilInit();
            Ilu.iluInit();
            Ilut.ilutInit();
        }


        public void Draw(Game game, LayoutManager manager)
        {
            foreach (var button in manager.Buttons.Values)
            {
                DrawButton(button.Position);
            }
        }

        public void DrawButton(PointF pos)
        {
            Gl.glColor3f(0, 0, 0);

            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2d(pos.X, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y);
            Gl.glVertex2d(pos.X + Consts.BUTTON_WIDTH, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glVertex2d(pos.X, pos.Y + Consts.BUTTON_HEIGHT);
            Gl.glEnd();
            Gl.glFlush();
        }
    }
}
