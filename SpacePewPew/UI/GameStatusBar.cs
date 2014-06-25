using System.Drawing;
using SpacePewPew.GameLogic;
using Tao.OpenGl;

namespace SpacePewPew.UI
{
    public class PlayerInfoStatusBar : UiElement
    {
        public PlayerInfoStatusBar()
        {
            Position = new PointF(0, 0);
        }

        public override void Draw()
        {
            //StatusBar background
            Gl.glColor3f(0.5f, 0.5f, 0.5f);

            Rect(0, 0, Consts.SCREEN_WIDTH, Consts.STATUS_BAR_HEIGHT);

            //PlayerName
            Gl.glColor3f(0, 0, 0);
            Rect(5, 1, 35, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(6, 5), PlayerInfo.Color.ToString());

            //stations
            Gl.glColor3f(0, 0, 0);
            Rect(45, 1, 60, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(46, 5), PlayerInfo.Ships.ToString());

            //ResourceGain
            Gl.glColor3f(0, 0, 0);
            Rect(70, 1, 85, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(71, 5), "+" + PlayerInfo.Ships);

            //ResourceCount
            Gl.glColor3f(0, 0, 0);
            Rect(95, 1, 110, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(96, 5), PlayerInfo.Money.ToString());

            //TimeLeft
            Gl.glColor3f(0, 0, 0);
            Rect(120, 1, 135, 6);
            Gl.glColor3f(1, 1, 0.3f);
            DrawString(new PointF(121, 5), Proxy.Proxy.GetInstance().ConvertTime(PlayerInfo.TimeLeft));

            Gl.glColor3f(0, 0, 0);
            Gl.glLineWidth(2);

            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex2d(0, 7);
            Gl.glVertex2d(Consts.SCREEN_WIDTH, 7);
            Gl.glEnd();
            Gl.glLineWidth(1);

            Gl.glColor3f(1, 1, 0.3f);
            Frame(5, 1, 35, 6);
            Frame(45, 1, 60, 6);
            Frame(70, 1, 85, 6);
            Frame(95, 1, 110, 6);
        }

        public override bool Click(PointF pos)
        {
            return false;
        }
    }
}
