using System;
using System.Windows.Forms;
using SpacePewPew.GameLogic;
using SpacePewPew.UI.Proxy;

namespace SpacePewPew
{
    public partial class MainForm : Form
    {
        public Game SpacePew;
        public static Drawer OglDrawer;
        public Proxy Proxy { get; set; }

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
            
            Proxy = Proxy.GetInstance();
            OglDrawer.Initialize();
            timer1.Enabled = true;
        }

        private void OGL_MouseClick(object sender, MouseEventArgs e)
        {
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
        }

        private void OGL_KeyDown(object sender, KeyEventArgs e)
        {
            Proxy.KeyDown(e.KeyCode);
        }

        private void OGL_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Proxy.KeyPress(e);
        }

        private void OGL_KeyUp(object sender, KeyEventArgs e)
        {
            //...
        }
    }
}
