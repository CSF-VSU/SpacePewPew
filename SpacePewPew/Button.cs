using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpacePewPew.UI;
using Tao.FreeGlut;
using Tao.OpenGl;
using System.Drawing;

namespace SpacePewPew
{
    abstract class Button : IUiElement
    {
        public Point Position { get; set; }
        public bool Visible { get; set; }
        


    } 
}
