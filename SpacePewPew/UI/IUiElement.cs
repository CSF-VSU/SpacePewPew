using System.Drawing;
using Tao.FreeGlut;
using Tao.OpenGl;

namespace SpacePewPew.UI
{
    public abstract class UiElement
    {
        public PointF Position { get; set; }

        public abstract void Draw();
        public abstract bool Click(PointF pos);

        public static void Rect(float x0, float y0, float x1, float y1)
        {
            Gl.glBegin(Gl.GL_POLYGON);
            Gl.glVertex2d(x0, y0);
            Gl.glVertex2d(x1, y0);
            Gl.glVertex2d(x1, y1);
            Gl.glVertex2d(x0, y1);
            Gl.glEnd();
        }

        public static void Frame(float x0, float y0, float x1, float y1)
        {
            Gl.glColor3f(1, 1, 0.3f);
            Gl.glLineWidth(2);
            Gl.glBegin(Gl.GL_LINE_STRIP);
            Gl.glVertex2d(x0, y0);
            Gl.glVertex2d(x1, y0);
            Gl.glVertex2d(x1, y1);
            Gl.glVertex2d(x0, y1);
            Gl.glVertex2d(x0, y0);
            Gl.glEnd();
            Gl.glLineWidth(1);
        }

        protected  void DrawString(PointF pos, string text)
        {
            Gl.glColor3f(1, 1, 0.3f);
            Gl.glRasterPos2d(pos.X, pos.Y);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_9_BY_15, text);
        }
    }
}
