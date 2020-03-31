using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEBUI.Models.Figures
{
    public class RectangleView : FigureView
    {
        public float Width { get; set; }
        public float Height { get; set; }
        public override string Info
        {
            get { return " Width = " + Width + " Height = " + Height; }
        }
    }
}