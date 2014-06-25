using System;
using System.Drawing;
using System.Windows.Forms;
using SpacePewPew.GameLogic;
using SpacePewPew.UI.Proxy;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        public Game SpacePew;
        public static Drawer OglDrawer;
        //public LayoutManager LayoutManager;
        public Proxy Proxy { get; set; }
        private PointF _mousePoint;

        public MainForm()
        { 
            InitializeComponent();
            OGL.InitializeContexts();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SpacePew = Game.Instance();
            OglDrawer = Drawer.Instance();
            SpacePew.Init(OglDrawer);
            
            //LayoutManager = LayoutManager.GetManager();
            Proxy = Proxy.GetInstance();
            OglDrawer.Initialize();
            timer1.Enabled = true;
        }


        private void OGL_MouseClick(object sender, MouseEventArgs e)
        {
            //var point = Additional.NewPoint(e.X, e.Y);
            /*switch (e.Button)
            {
                case MouseButtons.Left:
                   
                    /**/
                    
               /*     break;
                case MouseButtons.Right:
                    
                    break;
            }*/

            Proxy.GetInstance().OnClick(e.Location, e.Button);
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            Proxy.Tick();
            OGL.Invalidate();
        }

        private void OGL_MouseMove(object sender, MouseEventArgs e)
        {
            Proxy.GetMousePos(e.X, e.Y);
            
            /*var tmp = Additional.NewPoint(new PointF(e.X, e.Y));
            var tmp1 = OglDrawer.ScreenToCell(tmp);
            Text = String.Format("Mouse {0:00.###};  {1:00.###}    Cell: {2};{3}", tmp.X, tmp.Y, tmp1.X, tmp1.Y);*/
        }
    }
}
